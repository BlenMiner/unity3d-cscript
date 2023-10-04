#pragma once

struct Stack
{
	Stack(long long size = 512 * 512)
	{
		this->size = size;

		data = new long long[size];
		SP = size;
		SCOPE_SP = SP;
		IP = 0;
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
	}

	long long SCOPE_SP;
	long long SP;
	long long IP;
	long long* data;

private:
	long long size;
};
