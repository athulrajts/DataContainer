#pragma once
#include<string>
#include "DataContainer.h"

class DataContainerBuilderWrapper;

class DATACONTAINER_API DataContainerBuilder
{
public:
	static DataContainerBuilder* Create(std::string name = "");

	DataContainerBuilder* Data(std::string name, uint16_t value);
	DataContainerBuilder* Data(std::string name, uint32_t value);
	DataContainerBuilder* Data(std::string name, uint64_t value);
	DataContainerBuilder* Data(std::string name, int16_t value);
	DataContainerBuilder* Data(std::string name, int32_t value);
	DataContainerBuilder* Data(std::string name, int64_t value);
	DataContainerBuilder* Data(std::string name, std::string value);
	DataContainerBuilder* Data(std::string name, float value);
	DataContainerBuilder* Data(std::string name, double value);
	DataContainerBuilder* Data(std::string name, tm value);
	DataContainerBuilder* Data(std::string name, Color value);
	DataContainerBuilder* Data(std::string name, Point value);
	DataContainerBuilder* Data(std::string name, Duration value);
	DataContainerBuilder* Data(std::string name, bool value);
	DataContainerBuilder* Data(std::string name, char value);
	DataContainerBuilder* SubDataContainer(std::string name, DataContainerBuilder* innerBuilder);

	DataContainerBuilder* Data(std::string name, const char* value)
	{
		return Data(name, std::string(value));
	}

	DataContainer* Build();

private:
	DataContainerBuilder(std::string name);
	DataContainerBuilderWrapper* wrapper;

};

