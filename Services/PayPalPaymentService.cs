using System;
using System.Collections.Generic;
using PayPal.Api;

namespace cfunding.Services
{
    public class PayPalPaymentService
    {
        public int projectId { get; set;}
        public string pname { get; set;}
        public string donationAmount {get; set;}

        public PayPal.Api.Payment payment;
        public Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            //Adding Item Details like name, currency, price etc
            itemList.items.Add(new Item()
            {
                name = pname + " payment",
                currency = "USD",
                price = donationAmount,
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl + "&Cancel=false"
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = donationAmount
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = donationAmount, // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            var invoice_number = Convert.ToString((new Random()).Next(100000));
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Payment for project "+pname,
                invoice_number = invoice_number, //Generate an Invoice No
                amount = amount,
                item_list = itemList
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }


    }
}