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
	bool Get(std::string key, DataContainer& value);
	bool Get(std::string key, tm& value);
	bool Get(std::string key, Duration& value);
	bool Get(std::string key, Point& value);
	bool Get(std::string key, Color& value);

	void Put(std::string key, unsigned short value);
	void Put(std::string key, unsigned int value);
	void Put(std::string key, unsigned long long value);
	void Put(std::string key, short value);
	void Put(std::string key, int value);
	void Put(std::string key, long long value);
	void Put(std::string key, std::string value);
	void Put(std::string key, const char* value);
	void Put(std::string key, bool value);
	void Put(std::string key, float value);
	void Put(std::string key, double value);
	void Put(std::string key, DataContainer* value);
	void Put(std::string key, tm value);
	void Put(std::string key, Duration value);
	void Put(std::string key, Point value);
	void Put(std::string key, Color value);

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

