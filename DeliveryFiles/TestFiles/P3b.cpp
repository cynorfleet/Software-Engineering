3// Jonathan Bui
// CMPS 1044 - Dr. Johnson
// Program 3 - Switch
// 03/06/2017
// This program asks the user to enter a number
// from 1-10 and converts that number to a Roman numeral.

#include<iostream>
#include<string>

using namespace std;

int main()
{
	cout << "Jonathan Bui\n";
	cout << "CMPS 1044 - Dr. Johnson\n";
	cout << "Program 3 - Switch\n";
	cout << "03/06/2017\n";
	cout << "This program asks the user to enter a number\n";
	cout << "from 1 - 10 and converts that number to a Roman numeral.\n\n";

	int number;
	
		cout << "Enter a number within the range of 1-10: ";
		cin >> number;

	switch (number)
	{
	case 1:
		cout << "I\n";
		break;
	case 2:
		cout << "II\n";
		break;
	case 3:
		cout << "III\n";
		break;
	case 4:
		cout << "IV\n";
		break;
	case 5:
		cout << "V\n";
		break;
	case 6:
		cout << "VI\n";
		break;
	case 7:
		cout << "VII\n";
		break;
	case 8:
		cout << "VIII\n";
		break;
	case 9:
		cout << "IX\n";
		break;
	case 10:
		cout << "X\n";
		break;
	default:
		cout << "The number entered is outside the range \n";
	}


	system("pause");
	return 0;
}