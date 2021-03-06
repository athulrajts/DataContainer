#pragma once

#include <string>
#include <vector>
#include <msclr/gcroot.h>
#include "DataContainer.h"
#include "ValueConverter.h"

class DataContainerWrapper
{
public:
	DataContainerWrapper();
	DataContainerWrapper(KEI::Infrastructure::IDataContainer^ instance);

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

		KEI::Infrastructure::DataContainerExtensions::PutValue(instance, managedKey, ValueConverter<T>::GetManaged(value));
	}

	template <typename T>
	bool SetValue(std::string key, T value)
	{
		System::String^ managedKey = gcnew System::String(key.c_str());

		return instance->SetValue(managedKey, ValueConverter<T>::GetManaged(value));
	}

	KEI::Infrastructure::IDataContainer^ GetInstance()
	{
		return instance;
	}


private:
	msclr::gcroot<KEI::Infrastructure::IDataContainer^> instance;
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
		KEI::Infrastructure::IDataContainer^ dc = safe_cast<KEI::Infrastructure::IDataContainer^>(managed);
		DataContainerWrapper result;

		if (dc)
		{
			result = DataContainerWrapper(dc);
		}

		return result;
	}
};

