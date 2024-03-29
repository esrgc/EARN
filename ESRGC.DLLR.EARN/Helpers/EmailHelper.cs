﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ESRGC.DLLR.EARN.Controllers;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Helpers
{
  public class EmailHelper
  {
    public static bool SendEmailAlert(string emailAddress, string subject, string message) {
      try {
        //create smtp server instance using configuration settings
        SmtpClient mailClient = new SmtpClient();

        MailMessage email = new MailMessage();

        var fromAddress = ConfigurationManager.AppSettings["senderEmail"].ToString();
        var displayName = ConfigurationManager.AppSettings["senderName"].ToString();
        string bccAddress = ConfigurationManager.AppSettings["bccAddress"].ToString();

        email.Bcc.Add(new MailAddress(bccAddress));
        //set up email
        email.From = new MailAddress(fromAddress, displayName);
        //to emails
        email.To.Add(new MailAddress(emailAddress));
        //body
        email.Body = message;
        email.Subject = subject;
        //send
        mailClient.Send(email);
        return true;
      }
      catch {
        return false;
      }
    }
    public static bool SendEmailAlertMultiAddr(List<string> emailAddresses, string subject, string message) {
      try {
        //create smtp server instance using configuration settings
        SmtpClient mailClient = new SmtpClient();

        MailMessage email = new MailMessage();
        var fromAddress = ConfigurationManager.AppSettings["senderEmail"].ToString();
        var displayName = ConfigurationManager.AppSettings["senderName"].ToString();
        string bccAddress = ConfigurationManager.AppSettings["bccAddress"].ToString();
        email.Bcc.Add(new MailAddress(bccAddress));
        //set up email
        email.From = new MailAddress(fromAddress, displayName);

        //to emails
        foreach (var emailAddr in emailAddresses) {
          email.To.Add(new MailAddress(emailAddr));
        }
        //body
        email.Body = message;
        email.Subject = subject;
        //send
        mailClient.Send(email);
        return true;
      }
      catch {
        return false;
      }
    }

    public static void SendNotificationEmail(Notification model) {
      new EmailController().Notification(model).DeliverAsync();
    }
    public static void SendVerificationEmail(Account model) {
      new EmailController().EmailVerification(model).DeliverAsync();
    }
    public static void SendPasswordEmail(Account model) {
      new EmailController().ForgotPassword(model).DeliverAsync();
    }
    public static void SendReverificationEmail(Account model) {
      new EmailController().ReVerificationEmail(model).DeliverAsync();
    }
    public static void SendEmailMessage(Message model) {
      new EmailController().SendEmailMessage(model).DeliverAsync();
    }
    public static void SendCustomerEmailMessage(Message model) {
      new EmailController().SendCustomerEmail(model).DeliverAsync();
    }
    public static void SendAnnouncementEmail(Message model, List<string> emailList) {
      new EmailController().SendAnnouncementEmail(model, emailList).DeliverAsync();
    }
    public static void SendAnnouncementEmail(Message model, string emailAddress) {
      new EmailController().SendAnnouncementEmail(model, emailAddress).DeliverAsync();
    }
  }
}