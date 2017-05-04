//Richard Oliver
//M20242306
//Computer Science 1
//Dr. Johnson
//Program 2

//This is a catering service program that calculates balance due for catering  
//services rendered. This program accepts from the user, number of adults,   
//number of children, cost per adult meal, cost per desert, room fee, tax rate 
//and deposit, and calculates the cost for a child's meal, total for adult   
//meals, total for child meals, total for desert, tax amount, room fee, 
//subtotal and balance due and prints an invoice.

//preprocessor derivitives
#include <iostream>
#include <iomanip>
#include <fstream>

using namespace std;//indicaiting the name space that is being used

int main()
{
	ofstream outfile; //declaring an outfile
	outfile.open("output.txt"); //open output.txt for writing
	
	//declaration of decimal variables to store values for calculation and output
	double costPerChildMeal, costPerAdultMeal, costPerDesert, roomFee,
		totalCostOfAdultMeal, totalCostOfChildMeal, taxAmount, deposit,
		totalCostofDesert, totalFoodCost, totalAmount, subTotal, balanceDue,
		taxRate;

	//declaration of the integer variables to store values for calculation and 
	//output
	int noOfChildren, noOfAdults, totalPatrons;
	
	//header written to 'output.txt'
	outfile << "Richard Oliver \n";
	outfile << "M20242306 \n";
	outfile << "Computer Science 1 \n";
	outfile << "Dr. Johnson \n";
	outfile << "Program 2 \n\n";

	outfile << "Description: This is a catering service program that calcula-\n";
	outfile << "tes balance due for catering services rendered. This program \n";
	outfile << "accepts from the user, number of adults, number of children, \n";
	outfile << "cost per adult meal, cost per desert, room fee, tax rate and \n";
	outfile << "deposit, and calculates the cost for a child's meal, total \n";
	outfile << "for adult's meal, total for child meals, total for \n";
	outfile << "dessert, tax amount, room fee, subtotal and balance due \n";
	outfile << "and prints an invoice. \n\n";

	//header written to the screen
	cout << "Richard Oliver \n";
	cout << "M20242306 \n";
	cout << "Computer Science 1 \n";
	cout << "Dr. Johnson \n";
	cout << "Program 2 \n\n";

	cout << "Description: This is a catering service program that calculates \n";
	cout << "balance due for catering services rendered. This program accepts\n";
	cout << "from the user, number of adults, number of children, cost per \n";
	cout << "adult meal, cost per desert, room fee, tax rate and deposit, \n";
	cout << "and calculates the cost for a child's meal, total for adult \n";
	cout << "meals, total for child meals, total for dessert, tax amount, \n";
	cout << "room fee, subtotal and balance due and prints an invoice.\n\n";

	//prompts requesting user input
	cout << "Number of adults? ";
	cin >> noOfAdults;
	cout << "Number of children? ";
	cin >> noOfChildren;
	cout << "Cost per adult meal? ";
	cin >> costPerAdultMeal;
	cout << "Cost for a single dessert? ";
	cin >> costPerDesert;
	cout << "Room fee? ";
	cin >> roomFee;
	cout << "Tax rate? ";
	cin >> taxRate;
	cout << "Amount of deposit? ";
	cin >> deposit;

	//calculations
	costPerChildMeal = 0.60 * costPerAdultMeal;
	totalCostOfAdultMeal = costPerAdultMeal * noOfAdults;
	totalCostOfChildMeal = costPerChildMeal * noOfChildren;
	totalPatrons = noOfChildren + noOfAdults;
	totalCostofDesert = totalPatrons * costPerDesert;
	totalFoodCost = totalCostOfAdultMeal + totalCostOfChildMeal + 
		totalCostofDesert;
	taxAmount = totalFoodCost * taxRate;
	subTotal = totalFoodCost + taxAmount + roomFee;
	balanceDue = subTotal - deposit;

	//prints catering service invoice to the screan in table format
	cout << "\n\n";
	cout << "******* Catering Service ******* \n\n";
	cout << "Number of adults:      $" << setw(8) << noOfAdults << endl;
	cout << "Number of children:    $" << setw(8) << noOfChildren << endl;
	cout << fixed << setprecision(2);
	cout << "Cost per adult meal:   $" << setw(8) << costPerAdultMeal << endl;
	cout << "Cost per child meal:   $" << setw(8) << costPerChildMeal << endl;
	cout << "Cost per dessert:      $" << setw(8) << costPerDesert << endl;
	cout << "Room fee:              $" << setw(8) << roomFee << endl;
	cout << "Tax rate:              $" << setw(8) << taxRate << endl;
	cout << "\n";
	cout << "Total for adult meals: $" << setw(8) << totalCostOfAdultMeal 
		<< endl;
	cout << "Total for child meals: $" << setw(8) << totalCostOfChildMeal 
		<< endl;
	cout << "Total for dessert:     $" << setw(8) << totalCostofDesert << endl;
	cout << "Tax amount:            $" << setw(8) << taxAmount << endl;
	cout << "Room fee:              $" << setw(8) << roomFee << endl;
	cout << "\n";
	cout << "Subtotoal:             $" << setw(8) << subTotal << endl;
	cout << "Less deposit:          $" << setw(8) << deposit << endl;
	cout << "Balance due:           $" << setw(8) << balanceDue << endl;
	cout << "\n";
	
	//prints catering service invoice to 'output.txt' in table format
	outfile << "******* Catering Service ******* \n\n";
	outfile << "Number of adults:      $" << setw(8) << noOfAdults << endl;
	outfile << "Number of children:    $" << setw(8) << noOfChildren << endl;
	outfile << fixed << setprecision(2);
	outfile << "Cost per adult's meal: $" << setw(8) << costPerAdultMeal << endl;
	outfile << "Cost per child's meal: $" << setw(8) << costPerChildMeal << endl;
	outfile << "Cost per dessert:      $" << setw(8) << costPerDesert << endl;
	outfile << "Room fee:              $" << setw(8) << roomFee << endl;
	outfile << "Tax rate:              $" << setw(8) << taxRate << endl;
	outfile << "\n";
	outfile << "Total for adult meals: $" << setw(8) << totalCostOfAdultMeal 
		<< endl;
	outfile << "Total for child meals: $" << setw(8) << totalCostOfChildMeal 
		<< endl;
	outfile << "Total for dessert:     $" << setw(8) << totalCostofDesert 
		<< endl;
	outfile << "Tax amount:            $" << setw(8) << taxAmount << endl;
	outfile << "Room fee:              $" << setw(8) << roomFee << endl;
	outfile << "\n";
	outfile << "Subtotoal:             $" << setw(8) << subTotal << endl;
	outfile << "Less deposit:          $" << setw(8) << deposit << endl;
	outfile << "Balance due:           $" << setw(8) << balanceDue << endl;

	outfile.close();//closing the outfile
	system("pause");//pause the output on screen
	return 0;//release resouces back to the Operating System
}