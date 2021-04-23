#pragma once
#include<msclr/gcroot.h>
#include <msclr\marshal_cppstd.h>
#include <string>
#include <typeinfo>
#include "DataContainer.h"

#define ENABLE_CONVERTSION(_type)                                        \
template<>                                                               \
class ValueConverter<_type>                                              \
{                                                                        \
public:							                                         \
	static System::Object^ GetManaged(_type& unmanaged)                  \
    {                                                                    \
		return unmanaged;                                                \
    }                                                                    \
	static _type GetUnmanaged(System::Object^ managed)                   \
	{                                                                    \
		return (_type)managed;                                           \
	}                                                                    \
};                                                                       \

template <typename _type>
class ValueConverter
{
public:

	static System::Object^ GetManaged(_type& unmanaged) 
	{
		return gcnew System::Object();
	}

	static _type GetUnmanaged(System::Object^ managed) 
	{
		_type r{};
		return r;
	}
};

ENABLE_CONVERTSION(uint16_t)
ENABLE_CONVERTSION(uint32_t)
ENABLE_CONVERTSION(uint64_t)
ENABLE_CONVERTSION(int16_t)
ENABLE_CONVERTSION(int32_t)
ENABLE_CONVERTSION(int64_t)
ENABLE_CONVERTSION(bool)
ENABLE_CONVERTSION(float)
ENABLE_CONVERTSION(double)
ENABLE_CONVERTSION(char)

template <>
class ValueConverter<std::string>
{
public:
	static System::Object^ GetManaged(std::string& unmanaged)
	{
		return gcnew System::String(unmanaged.c_str());
	}

	static std::string GetUnmanaged(System::Object^ managed)
	{
		System::String^ s = safe_cast<System::String^>(managed);
		std::string result;

		if (s)
		{
			msclr::interop::marshal_context context;
			result = context.marshal_as<std::string>(s);
		}

		return result;
	}
};

template <>
class ValueConverter<Color>
{
public:
	static System::Object^ GetManaged(Color& unmanaged)
	{
		return gcnew System::Configuration::Color(unmanaged.r, unmanaged.g, unmanaged.b);
	}

	static Color GetUnmanaged(System::Object^ managed)
	{
		System::Configuration::Color^ c = safe_cast<System::Configuration::Color^>(managed);
		Color result{};

		if (c)
		{
			result.r = c->R;
			result.g = c->G;
			result.b = c->B;
		}

		return result;
	}
};

template <>
class ValueConverter<Point>
{
public:
	static System::Object^ GetManaged(Point& unmanaged)
	{
		return gcnew System::Configuration::Point(unmanaged.x, unmanaged.y);
	}

	static Point GetUnmanaged(System::Object^ managed)
	{
		System::Configuration::Point^ p = safe_cast<System::Configuration::Point^>(managed);
		Point result{};

		if (p)
		{
			result.x = p->X;
			result.y = p->Y;
		}

		return result;
	}
};

template <>
class ValueConverter<Duration>
{
public:
	static System::Object^ GetManaged(Duration& unmanaged)
	{
		return gcnew System::TimeSpan(unmanaged.days, unmanaged.hours, unmanaged.minutes, unmanaged.seconds, unmanaged.milliseconds);
	}

	static Duration GetUnmanaged(System::Object^ managed)
	{
		System::TimeSpan^ ts = safe_cast<System::TimeSpan^>(managed);
		Duration result{};

		if (ts)
		{
			result.days = ts->Days;
			result.hours = ts->Hours;
			result.minutes = ts->Minutes;
			result.seconds = ts->Seconds;
			result.milliseconds = ts->Milliseconds;
		}

		return result;
	}
};

template <>
class ValueConverter<tm>
{
public:
	static System::Object^ GetManaged(tm& unmanaged)
	{
		return System::DateTime(unmanaged.tm_year + 1900, unmanaged.tm_mon + 1, unmanaged.tm_mday, unmanaged.tm_hour, unmanaged.tm_min, unmanaged.tm_sec);
	}

	static tm GetUnmanaged(System::Object^ managed)
	{
		System::DateTime^ dt = safe_cast<System::DateTime^>(managed);
		tm result{};

		if (dt)
		{
			result.tm_year = dt->Year - 1900;
			result.tm_mon = dt->Month - 1;
			result.tm_mday = dt->Day;
			result.tm_hour = dt->Hour;
			result.tm_min = dt->Minute;
			result.tm_sec = dt->Second;
		}

		return result;
	}
};


