//Damien Moeller
//File: RecordStack.h
//RecordStack will create a dynamic array of records implimented as a stack.

#include <iostream>
#include <fstream>
#include <string>
#include <iomanip>
#include "Record.h"
#include "RecordStack.h"

using namespace std;

void MakeFile(ofstream& outfile, ifstream& infile);
void PrintSaleTot(ofstream& outfile, double total);
void OutStock(ofstream& outfile, int over_order);
string strfunct();
int main()
{
	RecordStack widget_stack;
	Record current;
	string stillworks = "";
	ofstream outfile;
	ifstream infile;
	infile.open("HWInput.txt");
	MakeFile(outfile, infile);
	while (!infile.eof())
	{
		current.GetRecord(infile);
		if (current.ReadType() == 'r')
		{
			current.PrintRecipt(outfile);
			widget_stack.Push(current);
		}
		if (current.ReadType() == 's')
		{
			//Seperate the recipts from the sales.
			cout << endl;
			outfile << endl;
			//inventory holds the record of the most recently gotten widgets.
			Record inventory;
			double total = 0;
			//Find out how many widgets are needed from the sales record.
			int sale_req = current.ReadQuan();
			cin >> stillworks;
			while (sale_req > 0)
			{
				//Make sure widgets are in stock, and keep getting them til order is filled.
				if (widget_stack.IsEmpty() == false)
				{
					widget_stack.Pop(inventory);
					//Get earliest recieved inventory.
					//Make sure there are widgets in the inventory.
					if (inventory.IsEmpty() == false)
					{
						//Sale the widgets in the inventory and keep track of the total.
						total = total + inventory.Sale(sale_req, outfile);
						//If there are still widgets in this record put them back on the stack.
						if (inventory.IsEmpty() == false)
						{
							widget_stack.Push(inventory);
						}
					}
				}
				//If all widgets are sold inform the buyer.
				if(widget_stack.IsEmpty() == true)
				{
					OutStock(outfile, sale_req);
					sale_req = 0;
				}
			}
			//Print the amount made from the sale.
			PrintSaleTot(outfile, total);
		}
	}
}

//Allow user to name the file to output to.
void MakeFile(ofstream& outfile, ifstream& infile)
{
	char file_name[20];
	cout << "Enter a name for a file to write to: ";
	infile >> file_name;
	cout << endl;
	outfile.open(file_name);
}

//Show the total after a sale.
void PrintSaleTot(ofstream& outfile, double total)
{
	cout << endl << "TOTAL FOR THIS SALE: $" << fixed << setprecision(2) << total << endl << endl;
	outfile << endl << "TOTAL FOR THIS SALE: $" << fixed << setprecision(2)<< total << endl << endl;
}

//Inform the user how many widgets cannot be sold.
void OutStock(ofstream& outfile, int over_order)
{
	cout << endl << over_order << " WIDGETS NOT AVAILABLE FOR SALE." << endl;
	outfile << endl << over_order << " WIDGETS NOT AVAILABLE FOR SALE." << endl;
}
