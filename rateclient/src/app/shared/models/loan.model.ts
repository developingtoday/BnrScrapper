export interface Loan {
  id: string;
  email: string;
  bankRate: number;
  bankMargin: number;
  sendEmail: boolean;
  ammount: number;
  rateofDatePayment: Date;
  months: number;
}
