package atm;

import bank.Bank;

public class Atm {
	private Bank bank;
	
	private void counting() {
		
	}
}

enum AtmState {
	CardEntry, PINEntry, Verification, ReturningCard, AmountEntry, Counting, Dispensing
}
