/*
Derek David
CMPS 1044 Johnson
02/28/17
Project 3-1 Roman Numerals
The user will enter a number between 1 and 10 and the program will
print out a numeral # using switch statements.
*/

#include<iostream>
#include<fstream>
#include<cstring>

using namespace std;

int main()
{
	int digit, count = 1;
	//bool quit = false;

	ofstream outfile;
	outfile.open("TimesNewRoman.txt"); //Opens TimesNewRoman.txt textfile.

	outfile << "Derek David\n";
	outfile << "CMPS 1044 Johnson\n";
	outfile << "02 / 28 / 17\n";
	outfile << "Project 3 - 1 Roman Numerals\n";
	outfile << "The user will enter a number between 1 and 10 and the program will ";
	outfile << "print out a numeral # using switch statements.\n\n";

	while (count < 12)
	{

		cout << "Please enter an integer between 1-10: \n";

		cin >> digit;

		outfile << "Trial " << count << endl;

		switch (digit)
		{
		case 1:
			outfile << 'I';
			
			break;
		case 2:
			outfile << "II";
			
			break;
		case 3:
			outfile << "III";
			
			break;
		case 4:
			outfile << "IV";
			
			break;
		case 5:
			outfile << 'V';
			
			break;
		case 6:
			outfile << "VI";
			
			break;
		case 7:
			outfile << "VII";
			
			break;
		case 8:
			outfile << "VIII";
			
			break;
		case 9:
			outfile << "IX";
			
			break;
		case 10:
			outfile << 'X';
			
			break;
		default:
			outfile << "Please enter a valid choice.\n";
			break;
		}

		outfile << endl << endl;

		count++;
		//goto Endwhile;
	}
	
		
	
		outfile.close(); // closes TimesNewRoman.txt
		
	system("pause"); // Please ignore the Red Squiggly, the program will compile and run fine.
	return 0;
}