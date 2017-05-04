// Program 3 - Number Converter
// 8 March 2017
// This program can convert the numbers
// 1 - 10 into Roman Numeral format.

#include <iostream>
using namespace std;

int main()
{
	int number;
	cout << "Program 3 - Number Converter" << endl;
	cout << "8 March 2017" << endl;
	cout << "This program can convert the numbers" << endl;
	cout << "1 - 10 into Roman Numeral format." << endl << endl;

	cout << "Enter a number between 1 and 10:" << endl;
	cin >> number;
	switch (number)
	{
	case 1:
		cout << "The Roman Numeral is I" << endl;
		break;
	case 2:
		cout << "The Roman Numeral is II" << endl;
		break;
	case 3:
		cout << "The Roman Numeral is III" << endl;
		break;
	case 4:
		cout << "The Roman Numeral is IV" << endl;
		break;
	case 5:
		cout << "The Roman Numeral is V" << endl;
		break;
	case 6:
		cout << "The Roman Numeral is VI" << endl;
		break;
	case 7:
		cout << "The Roman Numeral is VII" << endl;
		break;
	case 8:
		cout << "The Roman Numeral is VIII" << endl;
		break;
	case 9:
		cout << "The Roman Numeral is IX" << endl;
		break;
	case 10:
		cout << "The Roman Numeral is X" << endl;
		break;
	default:
		cout << "Invalid number" << endl;
	}
	system("pause");
	return 0;
}
