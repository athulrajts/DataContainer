#pragma once


#ifdef  DATACONTAINER_EXPORTS 
	#define DATACONTAINER_API __declspec(dllexport)  
#else
	#define DATACONTAINER_API __declspec(dllimport)  
#endif

