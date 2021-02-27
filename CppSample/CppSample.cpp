// CppSample.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "..\DataContainer.CLR\DataContainer.h"

int main()
{
    std::cout << "Hello Worlddsfds!\n";

    short v = 1;
    time_t t = time(0);
    tm date{};
    localtime_s(&date, &t);
    Duration duration{};
    duration.seconds = 72;

    Point p;
    p.x = 22;
    p.y = 34;

    Color c;
    c.r = 255;
    c.g = 123;
    c.b = 67;

    DataContainer dc;
    dc.Put("shortv", v);
    dc.Put("intv", 1);
    dc.Put("longv", (long long)1);
    dc.Put("ushortv", (unsigned short)1);
    dc.Put("uintv", (unsigned int)1);
    dc.Put("ulongv", (unsigned long long)1);
    dc.Put("stringv", "Hello Wolrd");
    dc.Put("doublev", 1.2);
    dc.Put("floatv", 1.2f);
    dc.Put("datev", date);
    dc.Put("timespanv", duration);
    dc.Put("pointv", p);
    dc.Put("colorv", c);


    DataContainer dc2;
    dc2.Put("stringv", "Hello Wolrd");
    dc2.Put("doublev", 1.2);
    dc2.Put("floatv", 1.2f);
    dc2.Put("dcv", &dc);
    dc2.SaveAsXml("Test.xml");
    dc2.SaveAsBinary("Test.dat");

    DataContainer dcXml = DataContainer::LoadFromBinary("Test.dat");

    auto keys = dcXml.GetKeys();

    DataContainer dcInner;
    bool result = dcXml.Get("dcv", dcInner);
    auto keys2 = dcInner.GetKeys();


    Color c2;
    Point p2;
    std::string str;
    dcInner.Get("colorv", c2);
    dcInner.Get("pointv", p2);

}