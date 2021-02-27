#pragma once

#include <string>
#include <vector>
#include <msclr/gcroot.h>
#include "DataContainer.h"

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

	bool Get(std::string key, std::string& value);
	bool Get(std::string key, bool& value);
	bool Get(std::string key, unsigned short& value);
	bool Get(std::string key, unsigned int& value);
	bool Get(std::string key, unsigned long long& value);
	bool Get(std::string key, short& value);
	bool Get(std::string key, int& value);
	bool Get(std::string key, long long& value);
	bool Get(std::string key, float& value);
	bool Get(std::string key, double& value);
	bool Get(std::string key, tm& value);
	bool Get(std::string key, Duration& value);
	bool Get(std::string key, Point& value);
	bool Get(std::string key, Color& value);
	bool Get(std::string key, DataContainerWrapper& value);

	void Put(std::string key, std::string value);
	void Put(std::string key, bool value);
	void Put(std::string key, unsigned short value);
	void Put(std::string key, unsigned int value);
	void Put(std::string key, unsigned long long value);
	void Put(std::string key, short value);
	void Put(std::string key, int value);
	void Put(std::string key, long long value);
	void Put(std::string key, float value);
	void Put(std::string key, double value);
	void Put(std::string key, System::DateTime^ value);
	void Put(std::string key, System::TimeSpan^ value);
	void Put(std::string key, KEI::Infrastructure::Point^ value);
	void Put(std::string key, KEI::Infrastructure::Color^ value);
	void Put(std::string key, DataContainerWrapper* value);


private:
	msclr::gcroot<KEI::Infrastructure::IDataContainer^> instance;
};

