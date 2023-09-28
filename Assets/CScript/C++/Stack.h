#pragma once

struct Stack
{
	Stack(long long size = 512 * 512)
	{
		this->size = size;

		data = new long long[size];
		registers = new long long[10];
		SP = size - 1;
		IP = 0;
	}

	~Stack()
	{
		delete[] data;
		delete[] registers;
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

	void SET_REGISTER(const int id, const long long value)
	{
		registers[id] = value;
	}

	long long GET_REGISTER(const int id)
	{
		return registers[id];
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
		return size - SP - 1;
	}

	void ResetSP()
	{
		SP = size - 1;
	}

	long long SP;
	long long IP;
	long long* data;
	long long* registers;

private:
	long long size;
};
