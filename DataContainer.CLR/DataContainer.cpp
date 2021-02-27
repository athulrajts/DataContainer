#include "pch.h"
#include "DataContainer.h"

#include "DataContainerWrapper.h"
#include <iostream>
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::Linq;

#pragma warning(push)
#pragma warning( disable : 4691 )

static std::string MarshalString(String^ managedString)
{
	msclr::interop::marshal_context context;
	return context.marshal_as<std::string>(managedString);
}

#pragma region Implementation

DataContainer::DataContainer()
{
	managed = new DataContainerWrapper();
}

DataContainer::DataContainer(DataContainerWrapper* wrapper)
{
	managed = wrapper;
}

DataContainer::~DataContainer()
{
	if (managed)
	{
		delete managed;
	}
}

bool DataContainer::Get(std::string key, std::string& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, int& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, long long& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, bool& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, unsigned short& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, unsigned int& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, unsigned long long& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, short& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, float& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, double& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, DataContainer& value)
{
	return managed->Get(key, *value.managed);
}

bool DataContainer::Get(std::string key, tm& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, Duration& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, Point& value)
{
	return managed->Get(key, value);
}

bool DataContainer::Get(std::string key, Color& value)
{
	return managed->Get(key, value);
}

void DataContainer::Put(std::string key, unsigned short value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, unsigned int value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, unsigned long long value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, short value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, int value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, long long value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, bool value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, float value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, double value)
{
	managed->Put(key, value);
};

void DataContainer::Put(std::string key, std::string value)
{
	managed->Put(key, value);
}

void DataContainer::Put(std::string key, const char* value)
{
	managed->Put(key, std::string(value));
}

void DataContainer::Put(std::string key, DataContainer* value)
{
	managed->Put(key, value->managed);
}

void DataContainer::Put(std::string key, tm value)
{
	System::DateTime^ dateTime = gcnew System::DateTime(value.tm_year + 1900, value.tm_mon + 1, value.tm_mday, value.tm_hour, value.tm_min, value.tm_sec);
	managed->Put(key, dateTime);
}

void DataContainer::Put(std::string key, Duration value)
{
	System::TimeSpan^ timeSpan = gcnew System::TimeSpan(value.days, value.hours, value.minutes, value.seconds, value.milliseconds);
	managed->Put(key, timeSpan);
}

void DataContainer::Put(std::string key, Point value)
{
	KEI::Infrastructure::Point^ p = gcnew KEI::Infrastructure::Point(value.x, value.y);
	managed->Put(key, p);
}

void DataContainer::Put(std::string key, Color value)
{
	KEI::Infrastructure::Color^ c = KEI::Infrastructure::Color(value.r, value.g, value.b);
	managed->Put(key, c);
}

std::vector<std::string> DataContainer::GetKeys()
{
	return managed->GetKeys();
}

DataContainer DataContainer::LoadFromXml(std::string path)
{
	return DataContainer(DataContainerWrapper::LoadFromXml(path));
}

DataContainer DataContainer::LoadFromBinary(std::string path)
{
	return DataContainer(DataContainerWrapper::LoadFromBinary(path));
}

bool DataContainer::SaveAsXml(std::string path)
{
	return managed->SaveAsXml(path);
}

bool DataContainer::SaveAsXml()
{
	return managed->SaveAsXml();
}

bool DataContainer::SaveAsBinary(std::string path)
{
	return managed->SaveAsBinary(path);
}

bool DataContainer::SaveAsBinary()
{
	return managed->SaveAsBinary();
}


#pragma endregion


#pragma region Wrapper

DataContainerWrapper::DataContainerWrapper()
{
	instance = gcnew KEI::Infrastructure::DataContainer();
}

DataContainerWrapper::DataContainerWrapper(KEI::Infrastructure::IDataContainer^ managed)
{
	instance = managed;
}

bool DataContainerWrapper::Get(std::string key, std::string& value)
{
	String^ val = gcnew String(value.c_str());
	bool retValue = instance->GetValue(gcnew String(key.c_str()), val);
	value = MarshalString(val);
	return retValue;
}

bool DataContainerWrapper::Get(std::string key, int& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, long long& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, bool& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, unsigned short& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, unsigned int& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, unsigned long long& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, short& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, float& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, double& value)
{
	return instance->GetValue(gcnew String(key.c_str()), value);
}

bool DataContainerWrapper::Get(std::string key, tm& value)
{
	System::Object^ obj = System::DateTime::Now;
	bool retValue = instance->GetValue(gcnew String(key.c_str()), obj);

	System::DateTime^ dt = safe_cast<System::DateTime^>(obj);

	if (dt)
	{
		value.tm_year = dt->Year - 1900;
		value.tm_mon = dt->Month - 1;
		value.tm_mday = dt->Day;
		value.tm_hour = dt->Hour;
		value.tm_min = dt->Minute;
		value.tm_sec = dt->Second;
	}

	return retValue;
}

bool DataContainerWrapper::Get(std::string key, Duration& value)
{
	System::Object^ obj = gcnew System::TimeSpan();
	bool retValue = instance->GetValue(gcnew String(key.c_str()), obj);

	System::TimeSpan^ ts = safe_cast<System::TimeSpan^>(obj);

	if (ts)
	{
		value.days = ts->Days;
		value.hours = ts->Hours;
		value.minutes = ts->Minutes;
		value.seconds = ts->Seconds;
		value.milliseconds = ts->Milliseconds;
	}

	return retValue;
}

bool DataContainerWrapper::Get(std::string key, Point& value)
{
	System::Object^ obj = gcnew KEI::Infrastructure::Point();
	bool retValue = instance->GetValue(gcnew String(key.c_str()), obj);

	KEI::Infrastructure::Point^ p = safe_cast<KEI::Infrastructure::Point^>(obj);

	if (p)
	{
		value.x = p->X;
		value.y = p->Y;
	}

	return retValue;
}

bool DataContainerWrapper::Get(std::string key, Color& value)
{
	System::Object^ obj = nullptr;
	bool retValue = instance->GetValue(gcnew String(key.c_str()), obj);
	KEI::Infrastructure::Color^ c = safe_cast<KEI::Infrastructure::Color^>(obj);

	if (c)
	{
		value.r = c->R;
		value.g = c->G;
		value.b = c->B;
	}

	return retValue;
}

bool DataContainerWrapper::Get(std::string key, DataContainerWrapper& value)
{
	KEI::Infrastructure::IDataContainer^ val = gcnew KEI::Infrastructure::DataContainer();
	bool retValue = instance->GetValue(gcnew String(key.c_str()), val);
	value.instance = val;
	return retValue;;
}

void DataContainerWrapper::Put(std::string key, std::string value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), gcnew String(value.c_str()));
}

void DataContainerWrapper::Put(std::string key, bool value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, unsigned short value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, unsigned int value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, unsigned long long value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, short value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, int value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, long long value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, float value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, double value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, System::DateTime^ value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, System::TimeSpan^ value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, KEI::Infrastructure::Point^ value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, KEI::Infrastructure::Color^ value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value);
}

void DataContainerWrapper::Put(std::string key, DataContainerWrapper* value)
{
	KEI::Infrastructure::DataContainerExtensions::PutValue(instance, gcnew String(key.c_str()), value->instance);
}

std::vector<std::string> DataContainerWrapper::GetKeys()
{
	std::vector<std::string> keys;

	for each (String ^ key in instance->GetKeys())
	{
		keys.push_back(MarshalString(key));
	}

	return keys;
}

DataContainerWrapper* DataContainerWrapper::LoadFromXml(std::string path)
{
	KEI::Infrastructure::IDataContainer^ dc = KEI::Infrastructure::DataContainer::FromXmlFile(gcnew String(path.c_str()));
	return new DataContainerWrapper(dc);
}

DataContainerWrapper* DataContainerWrapper::LoadFromBinary(std::string path)
{
	KEI::Infrastructure::IDataContainer^ dc = KEI::Infrastructure::DataContainer::FromBinaryFile(gcnew String(path.c_str()));

	if (dc == nullptr)
	{
		std::cout << "null";
	}

	return new DataContainerWrapper(dc);
}

bool DataContainerWrapper::SaveAsXml(std::string path)
{
	return instance->SaveAsXml(gcnew String(path.c_str()));
}

bool DataContainerWrapper::SaveAsXml()
{
	return instance->SaveAsXml();
}

bool DataContainerWrapper::SaveAsBinary(std::string path)
{
	return instance->SaveAsBinary(gcnew String(path.c_str()));
}

bool DataContainerWrapper::SaveAsBinary()
{
	return instance->SaveAsBinary();
}

#pragma endregion

#pragma warning(pop)
