// CppSample.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "..\DataContainer.CLR\DataContainerBuilder.h"

int main()
{

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


    DataContainer* built = DataContainerBuilder::Create("test")
        ->Data("shortv", v)
        ->Data("intv", 1)
        ->Data("longv", (int64_t)1)
        ->Data("ushortv", (uint16_t)1)
        ->Data("uintv", (uint32_t)1)
        ->Data("ulongv", (uint64_t)1)
        ->Data("doublev", 1.2)
        ->Data("floatv", 1.4f)
        ->Data("datev", date)
        ->Data("timev", duration)
        ->Data("pointv", p)
        ->Data("colorv", c)
        ->Data("stringv", "Hello World")
        ->SubDataContainer("dcv", DataContainerBuilder::Create()
            ->Data("doublev", 4.2)
            ->Data("floatv", 6.9)
            ->Data("stringv", "Blha"))
        ->Build();


    built->SaveAsXml("Test.xml");

    DataContainer dcXml = DataContainer::LoadFromBinary("Test.dat");

    auto keys = dcXml.GetKeys();

    DataContainer dcInner;
    bool result = dcXml.GetValue("dcv", dcInner);
    auto keys2 = dcInner.GetKeys();


    Color c2;
    Point p2;
    std::string str;
    dcInner.GetValue("colorv", c2);
    dcInner.GetValue("pointv", p2);

}