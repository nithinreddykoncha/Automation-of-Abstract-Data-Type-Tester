#include <iostream>
#include "debug.h"
#include "List.h"
using namespace std;
List::iterator::iterator(){current = nullptr; head = nullptr;}
// Move iterator to the next node
// Post increment
List::iterator List::iterator::operator++(Info){
   Entering()
   iterator iter;
   iter.current = current;      // copy the iterator
   iter.head = head;      // copy the iterator
   if (current != nullptr)
      current = current->next;  // increment the iterator;
   Leaving()
   return iter;
}
   
// Move iterator to next node; pre-increment
List::iterator & List::iterator::operator++(){
   Entering()
   if (current != nullptr)
      current = current->next;  // increment the iterator;
   Leaving()
   return *this;
}
   
// Move iterator n nodes forward
List::iterator & List::iterator::operator+(int n){
   Entering1(n)
   iterator *another = new iterator();
   another->head = head;
   another->current = current;
   int c = 0; 
   while (c < n && current != nullptr) {
      c++; another->current = another->current->next;
   }
   Leaving1(n)
   return *another;
}
   
// Return iterator n nodes before the current one
List::iterator & List::iterator::operator-(int n){ 
   Entering1(n)
   Node *t = head; // This is the sole reason we need head
   if (head == nullptr) {
      return *this;    // No error message
   }
   int p = 0;
   while (t != current) { p++; t=t->next; }// Figure out it position, 0-based
   p = p - n;       // Note p <0 neg if we move too far back
   current = head;  
   n = 0;
   iterator *another = new iterator();
   another->head = head; another->current = current;
   while (t != another->current && n < p) { // Nothing will happen if p <0
      n++; another->current = another->current->next; 
   }
   Leaving1(n)
   return *another;
}
   
// Compare two iterators
bool List::iterator::operator!= (const List::iterator& iter) {
   return (head != iter.head || current != iter.current);
}
   
// Return the information in the current node
Info List::iterator::operator*() {
   if (head == nullptr) {
      cerr << "Attempt to access an iterator not on a list" <<endl;
      exit(1);
   }
   return current->info;
}
   
// List() -- build an empty list
List::List(){ 
   Entering()
   head = tail = nullptr; length = 0;
}


// Copy nodes
Node *copy(Node *n) {
   Entering()
   if ( n == nullptr ) return n;
   Node *head = new Node(); head->info = n->info; head->next = copy(n->next);
   Leaving()
   return head;
}  

// copy constructor
List::List(const List &list){
   Entering()
   if (list.head == nullptr) {
      head = nullptr; tail = nullptr; length = 0;
   }
   else {
      head = copy(list.head);
      tail = head; 
      length = 0;
      while (tail->next != nullptr) tail = tail -> next;
   }
   Leaving()
}  

// Move constructor
List::List(List &&list){
   Entering()
   head = list.head;
   tail = list.tail;
   length = list.length; 
   Leaving()
}       

// Destructor
List::~List(){
   Entering()
   Node *t = head;  Node *n;
   while  (t != nullptr) {
      n = t; t=t->next; delete(n);
   }
   Leaving()
}                 
// Copy assignment
List & List::operator=(const List &list){
   Entering()
   if (list.head == nullptr) {
      head = nullptr; tail = nullptr; length = 0;
   }
   else {
      head = copy(list.head);
      tail = head; 
      length = 0;
      while (tail->next != nullptr) tail = tail -> next;
   }
   Leaving()
   return *this;
}  

// Move assignment
List & List::operator=(const List &&list){
   Entering()
   head = list.head;
   tail = list.tail;
   length = list.length; 
   Leaving()
   return *this;
} 

// Insert info at pos, shifting list[pos...] right
void List::insert(int pos, Info info){
   Entering2(pos,info)
   Msg(length)
   Node *t = head;
   if (pos <= 0) prepend(info);
   else if (pos >= length) append(info);
   else {
      int c = 1;
      while (t != nullptr && c < pos) {
         c++; t = t->next;
      }
      Node *n = new Node();
      n->info = info;
      n->next = t->next;
      t->next = n;
      length++;
   } 
   Leaving1(length)
}  

// Prepend info to the list
void List::prepend(Info info){
   Node * n = new Node();
   n->info = info;
   n->next = head;
   if (head == nullptr) tail = n;
   head = n;
   length++;
} 

// Append info to the list
void List::append(Info info){
   Entering1(info)
   Node * n = new Node();
   n->info = info;
   n->next = nullptr;
   if (head == nullptr) {
      head = n;
      tail = n;
   }
   else { 
      tail->next = n;
      tail = n;
   }
   length++;
   Leaving1(length) 
}  

// Delete the node at pos shifting pos shifting list[pos++ left]
void List::del(int pos){
   Entering1(pos)
   if (pos < 0) 
      cerr << "List::del -- attempt to delete at position " << pos << endl;
   else if (pos >= length) 
      cerr << "List::del -- attempt to delete at position " 
           << pos <<  " in a list of length " << length << endl;
   else if ( length == 1 ) {
      delete head;
      head = nullptr; 
      tail = nullptr;
   }

   // Note that beyond this point length > 1
   else if ( pos == 0 ) {    
      Node *t = head;
      head = head->next;
      delete head;
   }
   else {
      int c = 0;
      // Move to the node pos-1
      Node * t = head;
      while (c < pos) {
         c++; t = t->next;
      }
      Node *n = t;
      t->next = t->next->next;
      if (pos == length-1) // just deleted the tail
         tail = t;
   } 
   Leaving1(length)
}       


// True iff the list is empty
bool List::empty(){
   return length == 0;
}            

// Return info found at list[pos]
Info List::get(int pos){
   Entering1(pos) 
   if (pos <0 || pos >= length)  {
      cerr << "Attempt to access position " << pos << " in a list of length " << length <<endl;
   }
   // Move to the node pos-1
   Node *t = head;
   for (int p=0; p < pos; p++) {
      t = t->next;
   }
   Leaving1(t->info) 
   return t->info; 
}       
List::iterator List::begin(){
   iterator it;
   it.current = head;
   it.head = head;
   return it; 
}
List::iterator List::end(){
   iterator it;     //sets tail to nullptr
   it.head = head;  // tell it the first node
   return it;
}    

int List::size() { return length; }

Node* rev (Node *n) {
   Entering()
   if (n == nullptr || n->next == nullptr) return n;
   Node* t = rev(n->next);  // reverse the butfirst, return the head
   n->next->next = n;  // Make the tail point to n
   n->next = nullptr;
   Leaving()
   return t;
}


Node * rv (Node *n) {
   if (n == nullptr || n->next == nullptr) return n;
   Node *p = n, *q = p->next, *r = q->next;
   n->next = nullptr;  // Reverse the first node
   // p -- already reversed; q  is next to be reversed
   while (r != nullptr) {
      q->next = p;
      p = q; q = r; r = r->next;  // shift all 3 down
   } 
   // Now at the last two nodes 
   q->next = p;
   return q;
}
 
void List::reverse() {
   head = rv(head);
}

