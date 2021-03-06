#include "pch.h"
#include <iostream>
#include <msclr\marshal_cppstd.h>
#include "DataContainer.h"
#include "DataContainerWrapper.h"


using namespace System;
using namespace System::Linq;

#pragma warning(push)
#pragma warning( disable : 4691 )

static std::string MarshalString(String^ managedString)
{
	msclr::interop::marshal_context context;
	return context.marshal_as<std::string>(managedString);
}

#define ENABLE_TYPE_ALL(_type)\
bool DataContainer::GetValue(std::string key, _type& value)		\
{																\
	 return managed->GetValue(key, value);						\
}																\
void DataContainer::PutValue(std::string key, _type value)      \
{																\
	managed->PutValue(key, value);								\
}																\
bool DataContainer::SetValue(std::string key, _type value)		\
{																\
	return managed->SetValue(key, value);						\
}																\


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

ENABLE_TYPE_ALL(std::string)
ENABLE_TYPE_ALL(int16_t)
ENABLE_TYPE_ALL(int32_t)
ENABLE_TYPE_ALL(int64_t)
ENABLE_TYPE_ALL(uint16_t)
ENABLE_TYPE_ALL(uint32_t)
ENABLE_TYPE_ALL(uint64_t)
ENABLE_TYPE_ALL(bool)
ENABLE_TYPE_ALL(double)
ENABLE_TYPE_ALL(float)
ENABLE_TYPE_ALL(Color)
ENABLE_TYPE_ALL(Point)
ENABLE_TYPE_ALL(Duration)
ENABLE_TYPE_ALL(tm)

bool DataContainer::GetValue(std::string key, DataContainer& value)
{
	return managed->GetValue(key, *value.managed);
}


void DataContainer::PutValue(std::string key, const char* value)
{
	managed->PutValue(key, std::string(value));
}

void DataContainer::PutValue(std::string key, DataContainer* value)
{
	managed->PutValue(key, *value->managed);
}

bool DataContainer::SetValue(std::string key, const char* value)
{
	return managed->SetValue(key, std::string(value));
}

bool DataContainer::SetValue(std::string key, DataContainer* value)
{
	return managed->SetValue(key, *value->managed);
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

std::vector<std::string> DataContainerWrapper::GetKeys()
{
	std::vector<std::string> keys;

	for each (String ^ key in instance->GetKeys())
	{
		keys.push_back(ValueConverter<std::string>::GetUnmanaged(key));
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
