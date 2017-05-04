// Computer Science 1, Spring 17, Dr. Johnson
// Due 3/24/17
// This program will create a table displaying
// a salary which will double each day. The number
// of days depends on how many the user enters. 

#include<iomanip>
#include<iostream>

using namespace std;

int main() {
	int days, numCount = 10;
	double salDay = 5.12, salTot = 0;

	
	cin >> days;

	// Loop to make sure the variable "days" is a positive integer
	while (days <= 0)
	{
		cout << "You entered an invalid number.\n";
		cout << "Enter the number of days worked:   ";
		cin >> days;
	}

	cout << "   Day                Pay\n";
	cout << "-----------------------------------\n";
	cout << fixed << setprecision(2);

	// Loop to print table
	while (days != numCount) {

		cout << setw(5) << numCount << setw(23) << salDay << endl;

		salTot += salDay;
		salDay = salDay * 2;
		numCount++;
	}
	
	// code to print last line when days and numCount are equal
	cout << setw(5) << numCount << setw(23) << salDay << endl << endl;
	salTot += salDay;

	cout << "  TOTAL" << setw(9) << "$" << setw(12) << salTot << endl;

	system("pause");
	return 0;
}