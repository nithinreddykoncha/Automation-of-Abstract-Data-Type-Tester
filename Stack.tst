using namespace std;
#include <iostream>
#include <iomanip>
#include <string>
#include "Stack.h"
int p = 1;
int q = 2;
int r = 3;

Stack doit () {
    Stack *s = new Stack();
    return *s;
}

Stack s = doit();

states
Stack

A(s):
s.getSize() == 0, s.isEmpty()

B(s): 
s.getSize() == 1, s.top() == p, !s.isEmpty()

C(s): 
s.getSize() == 2, s.top() == q, !s.isEmpty()

D(s):
s.getSize() > 2, s.top() == r, !s.isEmpty()

Endstates

A(s) {s.push(p)}   B(s)
A(s) {s.pop(); writeln('Stack empty') } A(s)
A(s) {writeln('Stack empty'); s.top() } A(s)
B(s) {s.top()}   B(s)
B(s) {s.push(q)}  C(s)
B(s) {s.pop()}   A(s)
C(s) {s.push(r)}   D(s)
C(s) {s.pop()}   B(s)
C(s) {s.top()}   C(s)
D(s) {s.push(r)}   D(s)
D(s) {s.top()}   D(s)
D(s) && s.getSize()==3 {s.pop()}   C(s) 
D(s) && s.getSize()>3 {s.pop()}   D(s)
EndTransitions

