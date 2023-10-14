#pragma once
#include <exception>

#define SAFE_CHECKS

struct Stack
{
	Stack(long long size = 1024 * 1024)
	{
		this->size = size;
		data = new long long[size];
		ResetSP();
	}

	~Stack()
	{
		delete[] data;
	}

	long long PEEK()
	{
		return data[SP];
	}

	void DUP()
	{
		long long value = data[SP--];
		data[SP] = value;
	}

	void PUSH(const long long value)
	{
		data[--SP] = value;
	}

	void POP_DISCARD()
	{
		++SP;
	}

	long long POP()
	{
		return data[SP++];
	}

	long long GetPushedSize()
	{
		return size - SP;
	}

	void ResetSP()
	{
		SP = size;
		SCOPE_SP = SP - 1;
	}

	long long SCOPE_SP;
	long long SP;
	long long* data;

private:
	long long size;
};
