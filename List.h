typedef int Info;
struct Node {
   Info info;
   Node* next;
};
class List {
private:
   Node * head;
   Node * tail;
   int length;   
public:
   struct iterator {
       Node *current;  // References the current node in the list
       Node *head;     // References the first node of the list
       iterator();     // Creates an non-oriented iterator
       iterator operator++(Info info);   // postfix increment
       iterator & operator++();          // prefix  increment
       iterator & operator+(int n);      // Compute iterator at offset +n
       iterator & operator-(int n);      // Compute iterator at offset -n
       Info operator*();                 // Value referenced by current iterator
       bool operator!=(const iterator &iter); // Compare two iterators
   };

   List();
   List(const List &list);  // copy constructor
   List(List &&list);       // move constructor
   ~List();                 // destructor
   List & operator=(const List &list);  // copy assignment
   List & operator=(const List &&list); // move assignment
   void insert(int pos, Info info);  // insert info at pos, shifting list[pos...] right
   void prepend(Info info); //  same as insert(0,info)
   void append(Info info);  //  same as insert(size,info)
   void del(int pos);       // delete the node at pos shifting pos shifting list[pos++ left]
   bool empty();            // true iff the list is empty
   Info get(int pos);       // return info found at list[pos]
   int size();              // return the length of the list
   iterator begin();        // return an iterator to the beginning of the list
   iterator end();          // return an iterator to the end of the list
   void reverse();          // reverse the list in place
};

void testList() ;  // Test driver for testing list
