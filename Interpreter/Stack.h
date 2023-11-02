#pragma once
#include <exception>

#define SAFE_CHECKS

struct Stack
{
	Stack(int size = 1024 * 1024)
	{
		this->size = size;
		rawData = new char[size];
		ResetSP();
	}

	~Stack()
	{
		delete[] rawData;
	}

	template<class T>
	T PEEK()
	{
		char* dataPtr = rawData + SP;
		return *(T*)dataPtr;
	}

	template<class T>
	void REPLACE(const T value)
	{
		char* dataPtr = rawData + SP;
		*(T*)dataPtr = value;
	}

	template<class T>
	void PUSH(const T value)
	{
		SP -= sizeof(T);
		*(T*)(rawData + SP) = value;
	}

	void DISCARD(int count)
	{
		SP += count;
	}

	void RESERVE(int count)
	{
		SP -= count;
	}

	template<class T>
	T POP()
	{
		char* dataPtr = rawData + SP;
		SP += sizeof(T);

		return *(T*)dataPtr;
	}

	template<class T>
	T GET_VAR(long long offset)
	{
		char* dataPtr = rawData + SCOPE_SP - offset - sizeof(T);
		return *(T*)dataPtr;
	}

	template<class T>
	void SET_VAR(long long offset, T value)
	{
		char* dataPtr = rawData + SCOPE_SP - offset - sizeof(T);
		*(T*)dataPtr = value;
	}

	int GetPushedSize()
	{
		return size - SP;
	}

	void ResetSP()
	{
		SP = size;
		SCOPE_SP = SP;
	}

	int SCOPE_SP;
	int SP;
	char* rawData;

private:
	int size;
};
