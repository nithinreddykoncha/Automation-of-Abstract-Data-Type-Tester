/*
Array Implementation of Stacks
*/
#include <iostream>
using namespace std;

#include <iostream>
#include <string>
#include "stack.h"

Stack::Stack()
{
	info = new int[10];
	capacity = 10;
	size = 0;

}

Stack::Stack(int n)
{
	capacity = 2 * n;
	info = new int[capacity];
	size = 0;
}

Stack::Stack(const Stack &s) {
	capacity = 2 * s.size;
	info = new int[capacity];
	size = s.size;

	int i;
	for (int i = 0; i < size; i++) {
		info[i] = s.info[i];
	}
}

Stack & Stack::operator= (const Stack &s) {
	delete (info);
	capacity = 2 * s.size;
	info = new int[capacity];
	size = s.size;
	for (int i = 0; i < size; i++) {
		info[i] = s.info[i];
	}
	return *this;
}

Stack::Stack(Stack && s) {
	swap(info, s.info);
	swap(size, s.size);
}

Stack & Stack::operator= (Stack &&s) {
	swap(info, s.info);
	swap(size, s.size);
	return *this;
}

void Stack::push(const Info &e)
{
	if (size >= capacity) {
		int* temp = new int[2 * capacity];
		for (int i = 0; i < size; i++)  temp[i] = info[i];
	}
	info[size] = e;
	size++;
	cout << "Element pushed into the stack :: " << info[size - 1] << endl;
	cout << "Size of the stack :: " << size << endl;
}

void Stack::pop()
{
	if (size > 0) {
		cout << "Element popped out of the stack :: " << info[size - 1] << endl;
		size--;
		cout << "Size of the stack :: " << size << endl;
	}
	else
		cout << "void pop -- attempt to pop an empty stack" << endl;
}
Info Stack::top()
{
	if (size > 0) {
		return info[size - 1];
	}
	else {
		return Info();
	}
}
bool Stack::isEmpty()
{
	return size == 0;
}

int Stack::getSize()
{
	return size;
}

Stack::~Stack()
{
	delete[](info);
}
