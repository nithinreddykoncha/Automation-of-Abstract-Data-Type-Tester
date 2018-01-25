using namespace std;
#include <iostream>
#include <iomanip>
#include <string>
#include "List.h"
using namespace std;
int p = 1;
int q = 2;
int r = 3;

List doit () {
    List *l = new List();
    return *l;
}

List l = doit();

states
List

A(l):
l.size() == 0, l.empty()

B(l): 
l.size() == 1, !l.empty()

C(l):
l.size() == 2, !l.empty()

D(l):
l.size() > 2, !l.empty()

Endstates

A(l) {l.reverse()}   A(l)
A(l) {l.append(p)}   B(l)
A(l) {l.prepend(p)}   B(l)
A(l) {l.insert(p, 1)}   B(l)
A(l) {writeln('List empty'); l.del(0)}   A(l)
B(l) {l.reverse()}   B(l)
B(l) {l.append(q)}   C(l)
B(l) {l.prepend(q)}   C(l)
B(l) {l.insert(q, 1)}   C(l)
B(l) {l.del(0)}   A(l)
C(l) {l.reverse()}   C(l)
C(l) {l.append(r)}   D(l)
C(l) {l.prepend(r)}   D(l)
C(l) {l.insert(r, 1)}  D(l)
C(l) {l.del(1)}   B(l)
D(l) {l.reverse()}   D(l)
D(l) {l.append(r)}   D(l)
D(l) {l.prepend(r)}   D(l)
D(l) {l.insert(r, 1)}  D(l)
D(l) && l.size()==3 {l.del(1)}  C(l)
D(l) && l.size()>3 {l.del(1)}  D(l)

EndTransitions