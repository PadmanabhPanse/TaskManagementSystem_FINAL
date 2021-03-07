using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public partial class TMSDBContext : DbContext
    {
        public TMSDBContext()
        {
        }

        public TMSDBContext(DbContextOptions<TMSDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<Quotation> Quotations { get; set; }
        public virtual DbSet<QuoteTask> QuoteTasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<TaskDescription> TaskDetails { get; set; }
        public virtual DbSet<TaskHistory> TaskLogs { get; set; }
        public virtual DbSet<TaskOwner> TaskOwners { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskNotification> Notifications { get; set; }
        public virtual DbSet<TaskNotificationSetting> NotificationSettings { get; set; }
        public virtual DbSet<TaskReviewComments> ReviewComments { get; set; }
        public virtual DbSet<TaskStaffComments> StaffComments { get; set; }

        public virtual DbSet<StaffIncentive> StaffIncentives { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffIncentive>(entity =>
            {
                entity.HasKey(e => e.IncetiveId);

                entity.ToTable("Staff_Incentives");

                entity.Property(e => e.IncetiveId).HasColumnName("IncetiveId");
                entity.Property(e => e.StaffUserId).HasColumnName("StaffUserId").IsRequired();
                entity.Property(e => e.AssignedBy).HasColumnName("AssignedBy").IsRequired();

                entity.Property(e => e.AssignedDate)
                     .HasColumnName("AssignedDate")
                     .HasColumnType("datetime")
                     .IsRequired();

                entity.Property(e => e.TaskId).HasColumnName("Task_Id");
                entity.Property(e => e.IncentivesDecided).HasColumnName("IncentivesDecided").IsRequired();
                entity.Property(e => e.IncentivesPaid).HasColumnName("IncentivesPaid").IsRequired();
                entity.Property(e => e.IncentivesPaidDate)
                    .HasColumnName("IncentivesPaidDate")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Subscriber>(entity =>
            {

                entity.HasKey(e => e.SubscriberId);

                entity.ToTable("Subscriber");

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberId");
                entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionId").IsRequired();
                entity.Property(e => e.UserId).HasColumnName("UserId").IsRequired();

                entity.Property(e => e.TotalCost).HasColumnName("TotalCost").IsRequired();
                entity.Property(e => e.BalanceAmount).HasColumnName("BalanceAmount").IsRequired();
                entity.Property(e => e.ThresholdPrice).HasColumnName("ThresholdPrice");

                entity.Property(e => e.SubscriptionStartDate)
                      .HasColumnName("SubscriptionStartDate")
                      .HasColumnType("datetime")
                      .IsRequired();

                entity.Property(e => e.SubscriptionEndDate)
                    .HasColumnName("SubscriptionEndDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsLocked).HasColumnName("IsLocked");
            });
            modelBuilder.Entity<QuoteTask>(entity =>
            {
                entity.HasKey(e => e.TaskId);

                entity.ToTable("Quotation_Task");

                entity.Property(e => e.TaskId).HasColumnName("Quote_TaskId");

                entity.Property(e => e.QuoteId).HasColumnName("QuoteId");

                entity.Property(e => e.TaskName).HasColumnName("task_Name");

                entity.Property(e => e.TaskTitle).HasColumnName("task_Title");

                entity.HasOne(d => d.Quotation)
                 .WithMany(p => p.tasks)
                 .HasForeignKey(d => d.QuoteId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("FK_Quote_Qtask_fk0");
            });

            modelBuilder.Entity<Quotation>(entity =>
            {
                entity.HasKey(e => e.QuoteId);

                entity.ToTable("Quotation");

                entity.Property(e => e.QuoteId).HasColumnName("QuoteId");

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.Property(e => e.QuotationDate)
                    .HasColumnName("QuotationDate")
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.ConversionDate)
                    .HasColumnName("ConversionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title).HasColumnName("Title").IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").IsRequired();
                entity.Property(e => e.QuoteStatus).HasColumnName("QuoteStatus").IsRequired();
                entity.Property(e => e.RejectReason).HasColumnName("RejectReason");
                entity.Property(e => e.TotalCost).HasColumnName("TotalCost");

                entity.Property(e => e.Country).HasColumnName("Country");
                entity.Property(e => e.Currency).HasColumnName("Currency");
                entity.Property(e => e.TaxType).HasColumnName("TaxType");
                entity.Property(e => e.TaxRate).HasColumnName("TaxRate");


            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("AspNetUsers");

                entity.Property(e => e.UserId).HasColumnName("Id");

                entity.Property(e => e.Email).HasColumnName("Email");

                entity.Property(e => e.UserName).HasColumnName("UserName");

                entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("AspNetRoles");

                entity.Property(e => e.RoleId).HasColumnName("Id");

                entity.Property(e => e.RoleName).HasColumnName("Name");

            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.ToTable("AspNetUserRoles");

                entity.HasOne(ur => ur.User)
                .WithMany(u => u.userRoles)
                .HasForeignKey(ur => ur.UserId);

                //entity.HasOne(ur => ur.Role)
                //.WithMany(c => c.userRoles)
                //.HasForeignKey(ur => ur.Role);
            });



            modelBuilder.Entity<TaskReviewComments>(entity =>
            {
                entity.HasKey(e => e.TaskReviewCommentId);

                entity.ToTable("Task_Review_Comments");

                entity.Property(e => e.TaskReviewCommentId).HasColumnName("task_Review_Comment_Id");

                entity.Property(e => e.ReviewTaskId).HasColumnName("Review_Task_Id");

                entity.Property(e => e.TaskReviewActions).HasColumnName("task_Review_Actions");

                entity.Property(e => e.TaskReviewComment).HasColumnName("task_Review_Comment");

                entity.Property(e => e.TaskReviewDate)
                    .HasColumnName("task_Review_Date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ReviewCommentsTask)
                 .WithMany(p => p.reviews)
                 .HasForeignKey(d => d.ReviewTaskId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("FK_TASK_Review_FK0");
            });

            modelBuilder.Entity<TaskStaffComments>(entity =>
            {
                entity.HasKey(e => e.TaskCommentId);

                entity.ToTable("Task_Staff_Comments");

                entity.Property(e => e.TaskCommentId).HasColumnName("task_Comment_Id");

                entity.Property(e => e.CommentTaskId).HasColumnName("Comment_Task_Id");

                entity.Property(e => e.TaskComment).HasColumnName("task_Comment");

                entity.Property(e => e.TaskCommentDate)
                    .HasColumnName("task_Comment_Date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.StaffCommentsTask)
                 .WithMany(p => p.comments)
                 .HasForeignKey(d => d.CommentTaskId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("FK_TASK_Comment_FK0");
            });
            modelBuilder.Entity<TaskNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity.ToTable("Task_Notification");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("Notification_ID");

                entity.Property(e => e.IsRead)
                  .HasColumnName("Is_Read");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.EmailTime)
                    .HasColumnName("Email_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.NotificationDate)
                    .HasColumnName("Notification_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SettingId).HasColumnName("Setting_ID");

                entity.Property(e => e.SmsTime)
                    .HasColumnName("SMS_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.PopupDate)
                  .HasColumnName("Popup_date")
                  .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.OwnerId).HasColumnName("Owner_ID");

                entity.Property(e => e.ObjectId).HasColumnName("Object_ID").IsRequired();
                entity.Property(e => e.ObjectType).HasColumnName("ObjectType").IsRequired();

                entity.Property(e => e.IsPopup)
                 .HasColumnName("IsPopup");
            });

            modelBuilder.Entity<TaskNotificationSetting>(entity =>
            {
                entity.HasKey(e => e.SettingId);

                entity.ToTable("Task_Notification_Setting");

                entity.Property(e => e.SettingId)
                    .HasColumnName("Setting_ID");

                entity.Property(e => e.Sms).HasColumnName("SMS");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Dashboard).HasColumnName("Dashboard");
                entity.Property(e => e.Popup).HasColumnName("Popup");

                entity.Property(e => e.TaskChange)
                    .IsRequired()
                    .HasColumnName("Task_Change");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);


            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.ToTable("ClientMaster");

                entity.Property(e => e.ClientId).HasColumnName("client_ID");

                entity.Property(e => e.ClientComment).HasColumnName("client_comment");

                entity.Property(e => e.ClientCompanyName)
                    .IsRequired()
                    .HasColumnName("client_CompanyName");

                entity.Property(e => e.ClientContactDate)
                    .HasColumnName("client_ContactDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ClientContactPerson)
                    .IsRequired()
                    .HasColumnName("client_ContactPerson");

                entity.Property(e => e.ClientCreditId).HasColumnName("client_credit_id");

                entity.Property(e => e.ClientEmail).HasColumnName("client_Email");

                entity.Property(e => e.ClientIsLock).HasColumnName("client_Is_lock");

                entity.Property(e => e.ClientIsOrg).HasColumnName("client_Is_Org");

                entity.Property(e => e.ClientLocation).HasColumnName("client_Location");

                entity.Property(e => e.ClientOrgId).HasColumnName("client_Org_Id");

                entity.Property(e => e.ClientPaymentId).HasColumnName("client_payment_id");

                entity.Property(e => e.ClientPhone).HasColumnName("client_Phone");

                entity.Property(e => e.ClientRemarkId)
                    .HasColumnName("client_Remark_Id")
                    .HasMaxLength(10);

                entity.Property(e => e.ClientSubscriptionEnd)
                    .HasColumnName("client_Subscription_End")
                    .HasColumnType("datetime");

                entity.Property(e => e.ClientSubscriptionStart)
                    .HasColumnName("client_Subscription_Start")
                    .HasColumnType("datetime");

                entity.Property(e => e.ClinetInfoId).HasColumnName("clinet_info_id");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.ToTable("Document");

                entity.Property(e => e.DocumentId).HasColumnName("Document_Id");

                entity.Property(e => e.DocumentComments).HasColumnName("Document_Comments");

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentKeywordId).HasColumnName("Document_Keyword_Id");

                entity.Property(e => e.DocumentOwnerId).HasColumnName("Document_Owner_Id");

                entity.Property(e => e.DocumentPhysicalPath)
                    .IsRequired()
                    .HasColumnName("Document_PhysicalPath");

                entity.Property(e => e.DocumentTaskId).HasColumnName("Document_Task_Id");

                entity.HasOne(d => d.DocumentTask)
                    .WithMany(p => p.documents)
                    .HasForeignKey(d => d.DocumentTaskId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_t_Comment_t_Task");
            });

            modelBuilder.Entity<TaskDescription>(entity =>
            {
                entity.HasKey(e => e.TaskDescriptionId);

                entity.ToTable("Task_Desc");

                entity.Property(e => e.TaskDescriptionId).HasColumnName("task_Description_Id");

                entity.Property(e => e.TaskDescLine1).HasColumnName("task_Desc_Line1");

                entity.Property(e => e.TaskDescLine2).HasColumnName("task_Desc_Line2");

                entity.Property(e => e.TaskDescLine3).HasColumnName("task_Desc_Line3");

                entity.Property(e => e.TaskDescLine4).HasColumnName("task_Desc_Line4");

                entity.Property(e => e.TaskId).HasColumnName("task_Id");

                entity.Property(e => e.TaskKeywordId).HasColumnName("task_Keyword_Id");

                entity.Property(e => e.TaskQueryTypeId).HasColumnName("task_Query_type_id");
            });

            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.Property(e => e.TaskHistoryId).HasColumnName("task_History_Id");

                entity.Property(e => e.TaskHistoryComments).HasColumnName("taskHistory_Comments");

                entity.Property(e => e.TaskHistoryDocumentId).HasColumnName("taskHistory_Document_Id");

                entity.Property(e => e.TaskHistoryKeywords).HasColumnName("taskHistory_keywords");

                entity.Property(e => e.TaskHistoryOwnerId).HasColumnName("taskHistory_Owner_Id");

                entity.Property(e => e.TaskHistoryTaskAssignDate)
                    .HasColumnName("taskHistory_TaskAssignDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskHistoryTaskId).HasColumnName("taskHistory_Task_Id");

                entity.HasOne(d => d.TaskHistoryOwner)
                    .WithMany(p => p.TaskHistory)
                    .HasForeignKey(d => d.TaskHistoryOwnerId)
                    .HasConstraintName("FK_t_TaskHistory_t_Employee");

                entity.HasOne(d => d.TaskHistoryTask)
                    .WithMany(p => p.TaskHistory)
                    .HasForeignKey(d => d.TaskHistoryTaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_TaskHistory_t_Task");
            });

            modelBuilder.Entity<TaskOwner>(entity =>
            {
                entity.ToTable("Task_Owner");

                entity.Property(e => e.TaskOwnerId).HasColumnName("task_Owner_Id");

                entity.Property(e => e.TaskOwnerAddress)
                    .IsRequired()
                    .HasColumnName("task_Owner_Address");

                entity.Property(e => e.TaskOwnerAuthenticationModeFlag).HasColumnName("task_Owner_AuthenticationModeFlag");

                entity.Property(e => e.TaskOwnerDateOfBirth)
                    .HasColumnName("task_Owner_DateOfBirth")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskOwnerEmail).HasColumnName("task_Owner_Email");

                entity.Property(e => e.TaskOwnerIsLock).HasColumnName("task_Owner_IS_Lock");

                entity.Property(e => e.TaskOwnerJoinDate)
                    .HasColumnName("task_Owner_JoinDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskOwnerMacId).HasColumnName("task_Owner_MAC_ID");

                entity.Property(e => e.TaskOwnerName)
                    .IsRequired()
                    .HasColumnName("task_Owner_Name");

                entity.Property(e => e.TaskOwnerPhoneNo).HasColumnName("task_Owner_PhoneNo");
                entity.Property(e => e.UserId).HasColumnName("UserId");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => e.TaskId);

                entity.ToTable("t_Task");

                entity.HasIndex(e => new { e.TaskOwnerId, e.TaskPriority })
                    .HasName("UNIQUE_Priority")
                    .IsUnique();

                entity.Property(e => e.TaskId).HasColumnName("task_Id");

                entity.Property(e => e.TaskClientId).HasColumnName("task_Client_Id");

                entity.Property(e => e.TaskDescriptionId).HasColumnName("task_Description_Id");

                entity.Property(e => e.TaskDocumentId).HasColumnName("task_Document_id");

                entity.Property(e => e.TaskEstimateTime)
                    .HasColumnName("task_EstimateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskThresholdDate)
                    .HasColumnName("task_ThresholdDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskIsReopen).HasColumnName("task_Is_Reopen");

                entity.Property(e => e.TaskKeywordId).HasColumnName("task_Keyword_Id");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasColumnName("task_Name");

                entity.Property(e => e.TaskNotificationId).HasColumnName("task_Notification_Id");

                entity.Property(e => e.TaskOwner).HasColumnName("task_Owner");

                entity.Property(e => e.TaskAdminInstructions).HasColumnName("task_Admin_Instructions");

                entity.Property(e => e.TaskOwnerId).HasColumnName("task_Owner_Id");
                entity.Property(e => e.TaskStaffId).HasColumnName("task_Staff_Id");

                entity.Property(e => e.TaskPriority).HasColumnName("task_Priority");

                entity.Property(e => e.TaskQueryTypeId).HasColumnName("task_Query_type_id");

                entity.Property(e => e.TaskStartDate)
                    .HasColumnName("task_StartDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskStatusId).HasColumnName("task_Status_Id");

                entity.Property(e => e.TaskTitle)
                    .IsRequired()
                    .HasColumnName("task_Title");

                entity.HasOne(d => d.TaskOwnerNavigation)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.TaskOwnerId)
                    .HasConstraintName("FK_t_Task_t_Employee");

                entity.Property(e => e.UserId).HasColumnName("UserId");
            });
        }
    }
}
