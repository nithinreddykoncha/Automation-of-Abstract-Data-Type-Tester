#include <iostream>
#include <string>
#include "Pair.h"
using namespace std;
int m = 0;
int n = 6;
Pair P(m,n);

States
Pair

global(P):
P.GetFirst() == m, P.GetSecond() == n

A(P):
global(P)

B(P): 
global(P)

C(P): 
global(P)

EndStates

A(P) {P.SetFirst(m)}   B(P) && P.GetFirst() == m
B(P) {P.SetFirst(m)}   B(P) && P.GetFirst() == m
B(P) {P.SetSecond(n)}  C(P) && P.GetSecond() == n
B(P) {P.GetFirst()}   B(P) 
C(P) {P.SetFirst(m)}   B(P) && P.GetFirst() == m
C(P) {P.SetSecond(n)}  C(P) && P.GetSecond() == n
C(P) {P.GetFirst()}   B(P)
C(P) {P.GetSecond()}  C(P)

EndTransitions
