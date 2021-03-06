#pragma once
#include "DataContainer.CLR.h"
#include <string>
#include <vector>

class DataContainerWrapper;
struct Duration;
struct Point;
struct Color;

class DATACONTAINER_API DataContainer
{
public:
	DataContainer();
	DataContainer(DataContainerWrapper* wrapper);
	~DataContainer();

	std::vector<std::string> GetKeys();

	static DataContainer LoadFromXml(std::string path);
	static DataContainer LoadFromBinary(std::string path);
	bool SaveAsXml(std::string path);
	bool SaveAsXml();

	bool SaveAsBinary(std::string path);
	bool SaveAsBinary();

	bool GetValue(std::string key, std::string& value);
	bool GetValue(std::string key, bool& value);
	bool GetValue(std::string key, uint16_t& value);
	bool GetValue(std::string key, uint32_t& value);
	bool GetValue(std::string key, uint64_t& value);
	bool GetValue(std::string key, int16_t& value);
	bool GetValue(std::string key, int32_t& value);
	bool GetValue(std::string key, int64_t& value);
	bool GetValue(std::string key, float& value);
	bool GetValue(std::string key, double& value);
	bool GetValue(std::string key, DataContainer& value);
	bool GetValue(std::string key, tm& value);
	bool GetValue(std::string key, Duration& value);
	bool GetValue(std::string key, Point& value);
	bool GetValue(std::string key, Color& value);

	void PutValue(std::string key, uint16_t value);
	void PutValue(std::string key, uint32_t value);
	void PutValue(std::string key, uint64_t value);
	void PutValue(std::string key, int16_t value);
	void PutValue(std::string key, int32_t value);
	void PutValue(std::string key, int64_t value);
	void PutValue(std::string key, std::string value);
	void PutValue(std::string key, const char* value);
	void PutValue(std::string key, bool value);
	void PutValue(std::string key, float value);
	void PutValue(std::string key, double value);
	void PutValue(std::string key, DataContainer* value);
	void PutValue(std::string key, tm value);
	void PutValue(std::string key, Duration value);
	void PutValue(std::string key, Point value);
	void PutValue(std::string key, Color value);

	bool SetValue(std::string key, uint16_t value);
	bool SetValue(std::string key, uint32_t value);
	bool SetValue(std::string key, uint64_t value);
	bool SetValue(std::string key, int16_t value);
	bool SetValue(std::string key, int32_t value);
	bool SetValue(std::string key, int64_t value);
	bool SetValue(std::string key, std::string value);
	bool SetValue(std::string key, const char* value);
	bool SetValue(std::string key, bool value);
	bool SetValue(std::string key, float value);
	bool SetValue(std::string key, double value);
	bool SetValue(std::string key, DataContainer* value);
	bool SetValue(std::string key, tm value);
	bool SetValue(std::string key, Duration value);
	bool SetValue(std::string key, Point value);
	bool SetValue(std::string key, Color value);


private:
	DataContainerWrapper* managed;
};


struct DATACONTAINER_API Duration
{
public:
	int days;
	int hours;
	int minutes;
	int seconds;
	int milliseconds;
};

struct DATACONTAINER_API Point
{
public:
	double x;
	double y;
};

struct DATACONTAINER_API Color
{
public:
	unsigned char r;
	unsigned char g;
	unsigned char b;
};

