#include <iostream>
#include <cstdlib>
#include <ctime>
using namespace std;

int main()
{
	int x;	//Deklaracja zmiennych
	int i, j, k;
	int tab[8][8];
	int min = 5;	//2^5=32
	int max = 11;	//2^11=2048
	
	for(i=0; i!=8; i++)	//Czyszczenie tablicy
		for(j=0; j!=8; j++)
			tab[i][j]=1;
	
	srand(time(NULL));	//Tworzenie nowego ci¹gu liczb
	for(i=0; i!=8; i++)	//Odwo³anie do wymiaru tablicy
	{
		for(j=0; j!=8; j++)	//Odwo³anie do pozycji tablicy
		{
			x= (min + rand() % (max-min+1));	//Losowanie liczby z przedzia³u
			for(k=0; k!=x; k++)	//Potêgowanie dwójki
				tab[i][j]*=2;
			cout<<tab[i][j]<<"	";	//Zapisywanie pozycji wartoœci
		}
		cout<<endl;	//Rozpoczecie nowego wymiaru od nowej lini
		system("PAUSE");
		cout<<endl;
	}
	cout<<"Koniec tablicy"<<endl;
	system("PAUSE");
	return 0;
}

