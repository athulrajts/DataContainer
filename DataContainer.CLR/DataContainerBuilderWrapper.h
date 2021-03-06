#pragma once
#include <string>
#include <msclr/gcroot.h>
#include "DataContainerWrapper.h"

class DataContainerBuilderWrapper
{
public:

	static DataContainerBuilderWrapper* Create(std::string name = "")
	{
		return new DataContainerBuilderWrapper(name);
	}

	template<typename _type>
	DataContainerBuilderWrapper* Data(std::string name, _type value)
	{
		instance->Data(gcnew System::String(name.c_str()), ValueConverter<_type>::GetManaged(value));
		return this;
	}

	DataContainerBuilderWrapper* SubDataContainer(std::string name, DataContainerBuilderWrapper* innerBuilder)
	{
		if (innerBuilder)
		{
			instance->Data(gcnew System::String(name.c_str()), innerBuilder->Build()->GetInstance());
		}

		return this;
	}


	DataContainerWrapper* Build()
	{
		return new DataContainerWrapper(instance->Build());
	}

private:
	DataContainerBuilderWrapper(std::string name = "")
	{
		instance = KEI::Infrastructure::DataContainerBuilder::Create(gcnew System::String(name.c_str()));
	}

	msclr::gcroot<KEI::Infrastructure::DataContainerBuilder^> instance;
};