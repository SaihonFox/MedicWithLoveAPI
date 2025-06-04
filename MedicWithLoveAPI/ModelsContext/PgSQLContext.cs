using MedicWithLoveAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.ModelsContext;

public partial class PgSQLContext : DbContext
{
	public PgSQLContext() {}

	public PgSQLContext(DbContextOptions<PgSQLContext> options) : base(options) {}

	public virtual DbSet<Analysis> Analyses { get; set; }

	public virtual DbSet<AnalysisCategoriesList> AnalysisCategoriesLists { get; set; }

	public virtual DbSet<AnalysisCategory> AnalysisCategories { get; set; }

	public virtual DbSet<AnalysisOrder> AnalysisOrders { get; set; }

	public virtual DbSet<AnalysisOrderState> AnalysisOrderStates { get; set; }

	public virtual DbSet<Patient> Patients { get; set; }

	public virtual DbSet<PatientAnalysisCart> PatientAnalysisCarts { get; set; }

	public virtual DbSet<PatientAnalysisCartItem> PatientAnalysisCartItems { get; set; }

	public virtual DbSet<Request> Requests { get; set; }

	public virtual DbSet<RequestState> RequestStates { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserStatus> UserStatuses { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Analysis>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("analysis_pkey");

			entity.ToTable("analysis");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Biomaterial).HasColumnName("biomaterial");
			entity.Property(e => e.Description).HasColumnName("description");
			entity.Property(e => e.Name).HasColumnName("name");
			entity.Property(e => e.Preparation).HasColumnName("preparation");
			entity.Property(e => e.Price)
				.HasPrecision(10, 2)
				.HasColumnName("price");
			entity.Property(e => e.ResultsAfter).HasColumnName("results_after");
		});

		modelBuilder.Entity<AnalysisCategoriesList>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("analysis_categories_list_pkey");

			entity.ToTable("analysis_categories_list");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.AnalysisCategoryId).HasColumnName("analysis_category_id");
			entity.Property(e => e.AnalysisId).HasColumnName("analysis_id");

			entity.HasOne(d => d.AnalysisCategory).WithMany(p => p.AnalysisCategoriesLists)
				.HasForeignKey(d => d.AnalysisCategoryId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("analysis_categories_list_analysis_category_id_fkey");

			entity.HasOne(d => d.Analysis).WithMany(p => p.AnalysisCategoriesLists)
				.HasForeignKey(d => d.AnalysisId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("analysis_categories_list_analysis_id_fkey");
		});

		modelBuilder.Entity<AnalysisCategory>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("analysis_category_pkey");

			entity.ToTable("analysis_category");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Name).HasColumnName("name");
		});

		modelBuilder.Entity<AnalysisOrder>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("analysis_order_pkey");

			entity.ToTable("analysis_order");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.AnalysisDatetime)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("analysis_datetime");
			entity.Property(e => e.AnalysisOrderStateId)
				.HasDefaultValue(1)
				.HasColumnName("analysis_order_state_id");
			entity.Property(e => e.AtHome)
				.HasDefaultValue(false)
				.HasColumnName("at_home");
			entity.Property(e => e.Comment).HasColumnName("comment");
			entity.Property(e => e.PatientAnalysisCartId).HasColumnName("patient_analysis_cart_id");
			entity.Property(e => e.PatientId).HasColumnName("patient_id");
			entity.Property(e => e.RegistrationDate)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("registration_date");
			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.AnalysisOrderState).WithMany(p => p.AnalysisOrders)
				.HasForeignKey(d => d.AnalysisOrderStateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("analysis_order_analysis_order_state_id_fkey");

			entity.HasOne(d => d.PatientAnalysisCart).WithMany(p => p.AnalysisOrders)
				.HasForeignKey(d => d.PatientAnalysisCartId)
				.HasConstraintName("analysis_order_patient_analysis_cart_id_fkey");

			entity.HasOne(d => d.Patient).WithMany(p => p.AnalysisOrders)
				.HasForeignKey(d => d.PatientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("analysis_order_patient_id_fkey");

			entity.HasOne(d => d.User).WithMany(p => p.AnalysisOrders)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("analysis_order_user_id_fkey");
		});

		modelBuilder.Entity<AnalysisOrderState>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("analysis_order_state_pkey");

			entity.ToTable("analysis_order_state");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Name).HasColumnName("name");
		});

		modelBuilder.Entity<Patient>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("patient_pkey");

			entity.ToTable("patient");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Address).HasColumnName("address");
			entity.Property(e => e.Birthday).HasColumnName("birthday");
			entity.Property(e => e.Email).HasColumnName("email");
			entity.Property(e => e.Image).HasColumnName("image");
			entity.Property(e => e.Login).HasColumnName("login");
			entity.Property(e => e.Name).HasColumnName("name");
			entity.Property(e => e.Passport)
				.HasMaxLength(10)
				.HasColumnName("passport");
			entity.Property(e => e.Password)
				.HasDefaultValueSql("'123'::text")
				.HasColumnName("password");
			entity.Property(e => e.Patronym).HasColumnName("patronym");
			entity.Property(e => e.Phone)
				.HasMaxLength(11)
				.HasColumnName("phone");
			entity.Property(e => e.Sex)
				.HasMaxLength(10)
				.HasColumnName("sex");
			entity.Property(e => e.Surname).HasColumnName("surname");
		});

		modelBuilder.Entity<PatientAnalysisCart>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("patient_analysis_cart_pkey");

			entity.ToTable("patient_analysis_cart");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.PatientId).HasColumnName("patient_id");

			entity.HasOne(d => d.Patient).WithMany(p => p.PatientAnalysisCarts)
				.HasForeignKey(d => d.PatientId)
				.HasConstraintName("patient_analysis_cart_patient_id_fkey");
		});

		modelBuilder.Entity<PatientAnalysisCartItem>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("patient_analysis_cart_item_pkey");

			entity.ToTable("patient_analysis_cart_item");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.AnalysisId).HasColumnName("analysis_id");
			entity.Property(e => e.PatientAnalysisCartId).HasColumnName("patient_analysis_cart_id");
			entity.Property(e => e.ResultsDescription).HasColumnName("results_description");

			entity.HasOne(d => d.Analysis).WithMany(p => p.PatientAnalysisCartItems)
				.HasForeignKey(d => d.AnalysisId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("patient_analysis_cart_item_analysis_id_fkey");

			entity.HasOne(d => d.PatientAnalysisCart).WithMany(p => p.PatientAnalysisCartItems)
				.HasForeignKey(d => d.PatientAnalysisCartId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("patient_analysis_cart_item_patient_analysis_cart_id_fkey");
		});

		modelBuilder.Entity<Request>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("request_pkey");

			entity.ToTable("request");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.AnalysisDatetime)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("analysis_datetime");
			entity.Property(e => e.Comment).HasColumnName("comment");
			entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
			entity.Property(e => e.PatientAnalysisCartId).HasColumnName("patient_analysis_cart_id");
			entity.Property(e => e.PatientId).HasColumnName("patient_id");
			entity.Property(e => e.RequestChanged)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("request_changed");
			entity.Property(e => e.RequestSended)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("request_sended");
			entity.Property(e => e.RequestStateId)
				.HasDefaultValue(1)
				.HasColumnName("request_state_id");

			entity.HasOne(d => d.Doctor).WithMany(p => p.Requests)
				.HasForeignKey(d => d.DoctorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("request_doctor_id_fkey");

			entity.HasOne(d => d.PatientAnalysisCart).WithMany(p => p.Requests)
				.HasForeignKey(d => d.PatientAnalysisCartId)
				.HasConstraintName("request_patient_analysis_cart_id_fkey");

			entity.HasOne(d => d.Patient).WithMany(p => p.Requests)
				.HasForeignKey(d => d.PatientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("request_patient_id_fkey");

			entity.HasOne(d => d.RequestState).WithMany(p => p.Requests)
				.HasForeignKey(d => d.RequestStateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("request_request_state_id_fkey");
		});

		modelBuilder.Entity<RequestState>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("request_state_pkey");

			entity.ToTable("request_state");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Name).HasColumnName("name");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("user_pkey");

			entity.ToTable("user");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Birthday).HasColumnName("birthday");
			entity.Property(e => e.Image).HasColumnName("image");
			entity.Property(e => e.IsBlocked)
				.HasDefaultValue(false)
				.HasColumnName("is_blocked");
			entity.Property(e => e.Login).HasColumnName("login");
			entity.Property(e => e.Name).HasColumnName("name");
			entity.Property(e => e.Passport)
				.HasMaxLength(10)
				.HasColumnName("passport");
			entity.Property(e => e.Password)
				.HasDefaultValueSql("(123)::text")
				.HasColumnName("password");
			entity.Property(e => e.Patronym).HasColumnName("patronym");
			entity.Property(e => e.Phone)
				.HasMaxLength(11)
				.HasColumnName("phone");
			entity.Property(e => e.Post).HasColumnName("post");
			entity.Property(e => e.Surname).HasColumnName("surname");
		});

		modelBuilder.Entity<UserStatus>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("user_status_pkey");

			entity.ToTable("user_status");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.IsFired)
				.HasDefaultValue(false)
				.HasColumnName("is_fired");
			entity.Property(e => e.UserId).HasColumnName("user_id");

			entity.HasOne(d => d.User).WithMany(p => p.UserStatuses)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("user_status_user_id_fkey");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
