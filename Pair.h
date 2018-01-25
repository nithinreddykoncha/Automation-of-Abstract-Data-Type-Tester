#include<iostream>
#include<string>
using namespace std;


class Pair
{
    private: 
	    int f;
    	    int s;

public:    
Pair()
    {
        this->f = 0;
        this->s = 0;
    }
Pair(int f, int s)
    {
        this->f = f;
        this->s = s;
    }

    void SetFirst(int f)
    {
        this->f = f;
    }

    void SetSecond(int s)
    {
        this->s = s;
    }

    int GetFirst()
    {
        return f;
    }

    int GetSecond()
    {
        return s;
    }

	Pair & operator=(const Pair &P)
	{
		f = P.f;
		s = P.s;
		return *this;
	}
}
;