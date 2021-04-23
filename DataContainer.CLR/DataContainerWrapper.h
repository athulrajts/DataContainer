#pragma once

#include <string>
#include <vector>
#include <msclr/gcroot.h>
#include "DataContainer.h"
#include "ValueConverter.h"
#include "ChangeNotification.h"

class DataContainerWrapper
{
public:
	DataContainerWrapper();
	DataContainerWrapper(System::Configuration::IDataContainer^ instance);

	std::vector<std::string> GetKeys();

	static DataContainerWrapper* LoadFromXml(std::string path);
	static DataContainerWrapper* LoadFromBinary(std::string path);
	
	bool SaveAsXml(std::string path);
	bool SaveAsXml();

	bool SaveAsBinary(std::string path);
	bool SaveAsBinary();

	template <typename T>
	bool GetValue(std::string key, T& value)
	{
		System::String^ managedKey = gcnew System::String(key.c_str());

		System::Object^ managedValue = gcnew System::Object();
		
		if (instance->GetValue(managedKey, managedValue))
		{
			value = ValueConverter<T>::GetUnmanaged(managedValue);
			return true;
		}

		return false;
	}

	template <typename T>
	void PutValue(std::string key, T value)
	{
		System::String^ managedKey = gcnew System::String(key.c_str());

		System::Configuration::DataContainerExtensions::PutValue(instance, managedKey, ValueConverter<T>::GetManaged(value));
	}

	template <typename T>
	bool SetValue(std::string key, T value)
	{
		System::String^ managedKey = gcnew System::String(key.c_str());

		return instance->SetValue(managedKey, ValueConverter<T>::GetManaged(value));
	}

	System::Configuration::IDataContainer^ GetInstance()
	{
		return instance;
	}

	void AttachListener(std::function<void(std::string)> action)
	{
		unmanagedListner.SetCallBack(action);
	}

private:
	msclr::gcroot<System::Configuration::IDataContainer^> instance;
	msclr::gcroot<PropertyChangedListener^> listner;
	UnmanagedPropertyChangedListener unmanagedListner;
};

template <>
class ValueConverter<DataContainerWrapper>
{
public:
	static System::Object^ GetManaged(DataContainerWrapper& unmanaged)
	{
		return unmanaged.GetInstance();
	}

	static DataContainerWrapper GetUnmanaged(System::Object^ managed)
	{
		System::Configuration::IDataContainer^ dc = safe_cast<System::Configuration::IDataContainer^>(managed);
		DataContainerWrapper result;

		if (dc)
		{
			result = DataContainerWrapper(dc);
		}

		return result;
	}
};

