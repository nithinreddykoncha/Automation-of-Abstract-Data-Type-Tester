#ifndef STACK_H
#define STACK_H

/* 
   Array-based Implementation of Stacks
*/
#include <iostream>
using namespace std;


typedef int Info;  //Coordinates

class Stack {
private:
   int *info;
   int size;
   int capacity;
public:
   Stack();
   Stack(int);
   Stack (const Stack &s);
   Stack & operator= (const Stack &s);
   Stack (Stack && s);
   Stack & operator= (Stack &&s);
   void push (const Info &e);
   void pop ();
   Info top ();
   bool isEmpty ();
   int getSize();//added newly
  ~Stack(); 
};
#endif
