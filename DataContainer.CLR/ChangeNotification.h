#pragma once
#include <vector>
#include <functional>
#include <string>
#include "ValueConverter.h"

class UnmanagedPropertyChangedListener
{
public:
	void SetCallBack(std::function<void(std::string)> fn)
	{
		callBacks.push_back(fn);
	}

	void Notify(std::string prop)
	{
		for (auto& action : callBacks)
		{
			action(prop);
		}
	}

private:
	std::vector<std::function<void(std::string)>> callBacks;
};

ref class PropertyChangedListener
{
public:
	PropertyChangedListener(System::ComponentModel::INotifyPropertyChanged^ inpc, UnmanagedPropertyChangedListener* um)
	{
		inpc->PropertyChanged += gcnew System::ComponentModel::PropertyChangedEventHandler(this, &PropertyChangedListener::OnPropertyChanged);
		listner = um;
	}

	void OnPropertyChanged(System::Object^ sender, System::ComponentModel::PropertyChangedEventArgs^ e)
	{
		listner->Notify(ValueConverter<std::string>::GetUnmanaged(e->PropertyName));
	}

private:

	UnmanagedPropertyChangedListener* listner;
};