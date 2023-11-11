#pragma once
#include <exception>
#include <functional>

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
	inline T PEEK()
	{
		return *(T*)(rawData + SP);
	}

	template<class T>
	inline void DO_OPERATION_TOP_2(const std::function<T (T&a, T&b)>& func)
	{
		*(T*)(rawData + SP + sizeof(T)) = func(*(T*)(rawData + SP), *(T*)(rawData + SP + sizeof(T)));
		SP += sizeof(T);
	}

	template<class T>
	inline void DO_OPERATION_TOP_1(const std::function<T(T& a)>& func)
	{
		*(T*)(rawData + SP) = func(*(T*)(rawData + SP));
	}

	template<class T>
	inline void REPLACE(const T value)
	{
		char* dataPtr = rawData + SP;
		*(T*)dataPtr = value;
	}

	template<class T>
	inline void PUSH(const T value)
	{
		SP -= sizeof(T);
		*(T*)(rawData + SP) = value;
	}

	inline void DISCARD(long long count)
	{
		SP += (int)count;
	}

	inline void RESERVE(long long count)
	{
		SP -= (int)count;
	}

	inline void DISCARD(int count)
	{
		SP += count;
	}

	inline void RESERVE(int count)
	{
		SP -= count;
	}

	template<class T>
	inline T POP()
	{
		char* dataPtr = rawData + SP;
		SP += sizeof(T);

		return *(T*)dataPtr;
	}

	inline void PUSH_RETURN_INFO(long long ip, int sp)
	{
		SP -= sizeof(int) * 2;

		char* dataPtr = rawData + SP;

		*(int*)dataPtr = (int)ip;
		*(int*)(dataPtr + sizeof(int)) = sp;
	}

	inline void POP_RETURN_INFO(long long &ip, int &sp)
	{
		char* dataPtr = rawData + SP;

		ip = *(int*)dataPtr;
		sp = *(int*)(dataPtr + sizeof(int));

		SP += sizeof(int) * 2;
	}

	template<class T>
	inline T GET_VAR(long long offset)
	{
		return *(T*)(rawData + SCOPE_SP - offset - sizeof(T));
	}

	template<class T>
	inline void SET_VAR(long long offset, T value)
	{
		*(T*)(rawData + SCOPE_SP - offset - sizeof(T)) = value;
	}

	template<class T>
	inline void PUSH_VAR(long long offset)
	{
		SP -= sizeof(T);
		*(T*)(rawData + SP) = *(T*)(rawData + SCOPE_SP - offset - sizeof(T));
	}

	template<class T>
	inline void POP_TO_VAR(long long offset)
	{
		*(T*)(rawData + SCOPE_SP - offset - sizeof(T)) = *(T*)(rawData + SP);
		SP += sizeof(T);
	}

	inline int GetPushedSize()
	{
		return size - SP;
	}

	inline void ResetSP()
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
