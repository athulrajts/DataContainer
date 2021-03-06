#include "pch.h"
#include "DataContainerBuilder.h"
#include "DataContainerBuilderWrapper.h"

DataContainerBuilder::DataContainerBuilder(std::string name)
{
	wrapper = DataContainerBuilderWrapper::Create(name);
}

DataContainerBuilder* DataContainerBuilder::Create(std::string name)
{
	return new DataContainerBuilder(name);
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, uint16_t value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, uint32_t value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, uint64_t value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, int16_t value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, int32_t value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, int64_t value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, std::string value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, float value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, double value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, tm value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, Color value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, Point value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, Duration value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, bool value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::Data(std::string name, char value)
{
	wrapper->Data(name, value);

	return this;
}

DataContainerBuilder* DataContainerBuilder::SubDataContainer(std::string name, DataContainerBuilder* innerBuilder)
{
	wrapper->SubDataContainer(name, innerBuilder->wrapper);

	return this;
}

DataContainer* DataContainerBuilder::Build()
{
	return new DataContainer(wrapper->Build());
}
