// CMPS 1044-Johnson
// 3/7/17
// This program will show the user how many points they have earned in a
// book club based on the amount of books they have purchased in a month.

#include<iostream>
using namespace std;

int main()

	cout << "CMPS 1044 - Johnson" << endl;
	cout << "3/7/17" << endl;
	cout << "This program will show the user how many points they have earned in" << endl;
	cout << "a book club based on the amount of books they have purchased in a month." << endl << endl;
	int bookPur, points;
	int count = 1;
	// This wil make the program run 5 times so the user can enter differnt
	// amounts of books to see the amount of points received.
	while (count <= 5)
	{
		cout << "How many books did you purchase this month? ";
		cin >> bookPur;
		// This will calculate the amount of points based on the number of books entered.
		if (bookPur >= 4)
			points = 60;
		else if (bookPur == 3)
			points = 30;
		else if (bookPur == 2)
			points = 15;
		else if (bookPur == 1)
			points = 5;
		else if (bookPur == 0)
			points = 0;
		// This will print the number of books and the points associated with the books.
		cout << "Books purchased: " << bookPur << endl;
		cout << "Points Earned: " << points << endl << endl;
		count++;
	}
	system("pause");
	return 0;
}