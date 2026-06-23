using Hospital_Management_System.Models;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Hospital_Management_System
{
    public class Program
    {
        public static HospitalContext context = new HospitalContext
        {
            Patients = new List<Patient>(),
            Doctors = new List<Doctor>(),
            Appointments = new List<Appointment>(),
            MedicalRecords = new List<MedicalRecord>(),
            AvailableSlots = new List<AvailableSlot>()
        }; 

        public static void PatientRegistration() // 01
        {

            Console.WriteLine("Enter patient Name");
            string patientName = Console.ReadLine();

            Console.WriteLine("Enter patient Age");
            int patientAge = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter patient Gender");
            string patientGender = Console.ReadLine();

            Console.WriteLine("Enter patient Phone");
            string patientPhone = Console.ReadLine();

            Console.WriteLine("Enter patient Email");
            string patientEmail = Console.ReadLine();

            Console.WriteLine("Enter patient Blood Type");
            string patientBloodType = Console.ReadLine();

            int patientId = context.Patients.Count + 1;

            context.Patients.Add(new Patient
            {
                patientId = patientId,
                patientName = patientName,
                patientAge = patientAge,
                patientGender = patientGender,
                patientPhone = patientPhone,
                patientEmail = patientEmail,
                patientBloodType = patientBloodType

            });
            //patients.Add(new Patient(patientId, patientName, patientAge, patientGender, patientPhone, patientEmail, patientBloodType));

            Console.WriteLine($"Patient registered successfully.. ID = {patientId}");

        }

        public static void AddNewDoctor() // 02 
        {

            Console.WriteLine("Enter doctor Name");
            string doctorName = Console.ReadLine();

            Console.WriteLine("Enter doctor Specialization");
            string doctorSpecialization = Console.ReadLine();

            Console.WriteLine("Enter doctor Phone");
            string doctorPhone = Console.ReadLine();

            Console.WriteLine("Enter doctor Email");
            string doctorEmail = Console.ReadLine();

            Console.WriteLine("Enter consultation Fee");
            decimal consultationFee = decimal.Parse(Console.ReadLine()); // رسوم الاستشاره

            int doctorId = context.Doctors.Count + 1;

            context.Doctors.Add(new Doctor
            {
                doctorId = doctorId,
                doctorName = doctorName,
                doctorSpecialization = doctorSpecialization,
                doctorPhone = doctorPhone,
                doctorEmail = doctorEmail,
                consultationFee = consultationFee

            });

            Console.WriteLine($"Doctor registered successfully.. ID: {doctorId}");
        }

        public static void ViewAllPatients() // 03 
        {
            if (context.Patients.Count == 0)
            {
                Console.WriteLine("No Patients Found");
                return;
            }

            //foreach (Patient patient in context.Patients) 
            //{
            //    Console.WriteLine($"patient Id: {patient.patientId}");
            //    Console.WriteLine($"patient Name: {patient.patientName}");
            //    Console.WriteLine($"patient Age: {patient.patientAge}");
            //    Console.WriteLine($"patient Gender: {patient.patientGender}");
            //    Console.WriteLine($"patient Phone: {patient.patientPhone}");
            //    Console.WriteLine($"patient Email: {patient.patientEmail}");
            //    Console.WriteLine($"patient Blood Type: {patient.patientBloodType}");
            //    Console.WriteLine("===============================");
            //}

            foreach (Patient patient in context.Patients)
            {
                patient.printInfo();
            }

        }

        public static void ViewAllDoctorsbySpecialization() // 04 
        {
            Console.WriteLine("Enter Specialization:");
            string doctorSpecialization = Console.ReadLine();

            #region Previous Code 
            //bool found = false;
            //foreach (Doctor doctor in context.Doctors)
            //{
            //    if (doctor.doctorSpecialization == doctorSpecialization)
            //    {
            //        Console.WriteLine($"Doctor Name: {doctor.doctorName}");
            //        Console.WriteLine($"consultation Fee: {doctor.consultationFee}");
            //        Console.WriteLine("===============================");

            //        found = true;
            //    }
            //}

            //if (found == false)
            //{
            //    Console.WriteLine("No Doctors Found");
            //} 
            #endregion

            List<Doctor> drSpecialization = context.Doctors.Where(d => d.doctorSpecialization == doctorSpecialization) // where تطلع أكثر من شخص أو تطلع مجموعه من التخصص المطلوب
                                                           .ToList();

            if (drSpecialization.Count == 0)
            {
                Console.WriteLine("No Doctors Found");
                return;
            }

            foreach (Doctor doctor in drSpecialization)
            {
                Console.WriteLine($"Doctor Name: {doctor.doctorName}");
                Console.WriteLine($"Consultation Fee: {doctor.consultationFee}");
                Console.WriteLine("==========================");
            }
        }

        public static void AddAvailableSlot() // 05 
        {

            if (context.Doctors.Count == 0)
            {
                Console.WriteLine("No doctors in the system yet.");
                return;
            }

            context.Doctors.ForEach(d => Console.WriteLine($"ID: {d.doctorId} | Name: {d.doctorName} | Specialization: {d.doctorSpecialization}"));

            Console.WriteLine("Enter doctor ID: ");
            int doctorId = int.Parse(Console.ReadLine());

            bool result = context.Doctors.Any(d => d.doctorId == doctorId);

            if (result == false)
            {
                Console.WriteLine("No doctor found with ID");
                return;
            }

            Console.WriteLine("Enter solt date: ");
            string slotDate = Console.ReadLine();

            Console.WriteLine("Enter solt time: ");
            string slotTime = Console.ReadLine();

            int slotId = context.AvailableSlots.Count + 1;

            context.AvailableSlots.Add(new AvailableSlot
            {
                slotId = slotId,
                doctorId = doctorId,
                slotDate = slotDate,
                slotTime = slotTime,
                isBooked = false // الموعد غير محجوز حالياً، يعني يقدر المريض الحجز 
            });

            Console.WriteLine("Slot Added Successfully");
        }

        public static void BookAppointment() // 06  
        {

            Console.WriteLine("Enter Patient ID:");
            int patientId = Convert.ToInt32(Console.ReadLine());

            Patient patient = context.Patients.FirstOrDefault(p => p.patientId == patientId);

            if (patient == null)
            {
                Console.WriteLine("Patient not found");
                return;
            }

            ViewAllDoctorsbySpecialization();

            Console.WriteLine("Enter Doctor ID to book with:");
            int doctorId = Convert.ToInt32(Console.ReadLine());

            Doctor doctor = context.Doctors.FirstOrDefault(d => d.doctorId == doctorId);

            if (doctor == null)
            {
                Console.WriteLine("Doctor not found");
                return;
            }

            #region Previous Code 
            //bool slotFound = false;

            //foreach (AvailableSlot slot in context.AvailableSlots) // AvailableSlot ==> slot 
            //{
            //    if (slot.doctorId == doctorId && slot.isBooked == false)
            //    {
            //        Console.WriteLine($"Slot ID: {slot.slotId}");
            //        Console.WriteLine($"Doctor ID: {slot.doctorId}");
            //        Console.WriteLine($"{slot.slotDate} , {slot.slotTime}");
            //        Console.WriteLine("===============================");

            //        slotFound = true;
            //    }
            //}

            //if (slotFound == false)
            //{
            //    Console.WriteLine("No Available Slots");
            //    return;
            //} 
            #endregion

            List<AvailableSlot> availableSlots = context.AvailableSlots.Where(s => s.doctorId == doctorId && s.isBooked == false)
                                                                       .ToList(); // يشوف إذا متاح حجز فارغ باسم الدكتور  

            if (availableSlots.Count == 0)
            {
                Console.WriteLine("No Available Slots");
                return;
            }

            Console.WriteLine($"Available Slots for Dr. {doctor.doctorName}: ");

            availableSlots.ForEach(s => Console.WriteLine($"Solt ID: {s.slotId} | Date: {s.slotDate} | Time: {s.slotTime}"));

            Console.WriteLine("Enter solt ID to book:");
            int slotId = Convert.ToInt32(Console.ReadLine());

            AvailableSlot selectSlot = availableSlots.FirstOrDefault(s => s.slotId == slotId);

            if (selectSlot == null)
            {
                Console.WriteLine("Slot not found or already booked.");
                return;
            }

            int appointmentId = context.Appointments.Count + 1;

            context.Appointments.Add(new Appointment
            {
                appointmentId = appointmentId,
                patientId = patientId,
                doctorId = doctorId,
                appointmentDate = selectSlot.slotDate,
                appointmentTime = selectSlot.slotTime,
                status = "Booked"
            });

            #region Previous Code 
            //AvailableSlot availableSlot = null; // متغير فارغ عشان يخزن بعدين الsolt اللي اختاره المريض 

            //foreach (AvailableSlot slot in context.AvailableSlots)
            //{
            //    if (slot.slotId == slotId) // 1==2 X , 2==2 /
            //    {
            //        availableSlot = slot;
            //        break;
            //    }
            //}

            //if (availableSlot == null)
            //{
            //    Console.WriteLine("Slot Not Found");
            //    return;
            //} 
            #endregion

            selectSlot.isBooked = true; // تم حجز موعد المريض 

            Console.WriteLine($"Appointment Booked Successfully .. Appointment ID: {appointmentId} | Date: {selectSlot.slotDate} | Time: {selectSlot.slotTime}");
        }

        public static void CancelAppointment() // 07 
        {
            Console.WriteLine("Enter Appointment ID:");
            int appointmentId = Convert.ToInt32(Console.ReadLine());

            #region Previous Code 
            //Appointment appointment = null;

            //foreach (Appointment item in context.Appointments)
            //{
            //    if (item.appointmentId == appointmentId)
            //    {
            //        appointment = item;
            //        break;
            //    }
            //} 
            #endregion

            Appointment appointment = context.Appointments.FirstOrDefault(a => a.appointmentId == appointmentId); //  نبحث عن أول عنصر يحقق الشرط
                                                                                                                  // إذا الموعد المحجوز يطابق اللي أريد ألغيه
            if (appointment == null)
            {
                Console.WriteLine("Appointment Not Found");
                return;
            }

            if (appointment.status == "Cancelled")
            {
                Console.WriteLine("Appointment Already Cancelled");
                return;
            }

            if (appointment.status == "Completed")
            {
                Console.WriteLine("Cannot cancel a completed appointment.");
                return;
            }

            #region Previous Code 
            //foreach (AvailableSlot slot in context.AvailableSlots)
            //{
            //    if (slot.doctorId == appointment.doctorId &&
            //        slot.slotDate == appointment.appointmentDate &&
            //        slot.slotTime == appointment.appointmentTime)
            //    {
            //        slot.isBooked = false;
            //    }
            //} 
            #endregion

            AvailableSlot availableSlot = context.AvailableSlots.FirstOrDefault(solt => solt.doctorId == appointment.doctorId &&
                                                                                        solt.slotDate == appointment.appointmentDate &&
                                                                                        solt.slotTime == appointment.appointmentTime);

            if (availableSlot != null) // إذا كان موجود الشاغر 
            {
                availableSlot.isBooked = false;
            }

            appointment.status = "Cancelled";

            Console.WriteLine($"Appointment {appointmentId} has been Cancelled");
        }

        public static void CreateMedicalRecord() // 08 
        {

            Console.WriteLine("Enter Appointment ID:");
            int appointmentId = Convert.ToInt32(Console.ReadLine());

            #region Previous Code 
            //Appointment appointment = null;

            //foreach (Appointment item in context.Appointments) // يمر على جميع المواعيد المتخزنه قبل واحد واحد لين يحصل الموعد المطلوب
            //{
            //    if (item.appointmentId == appointmentId)
            //    {
            //        appointment = item; // هل يتطابقان 2=2
            //        break;
            //    }
            //} 
            #endregion

            Appointment appointment = context.Appointments.FirstOrDefault(a => a.appointmentId == appointmentId);

            if (appointment == null)
            {
                Console.WriteLine("Appointment Not Found");
                return;
            }

            if (appointment.status == "Cancelled")
            {
                Console.WriteLine("Cannot create a medical record for a cancelled appointment.");
                return;
            }

            if (appointment.status == "Completed")
            {
                Console.WriteLine("A medical record already exists for this appointment.");
                return;
            }

            decimal fee = context.Doctors.Where(d => d.doctorId == appointment.doctorId).Select(d => d.consultationFee).FirstOrDefault();

            Console.WriteLine("Enter Diagnosis:");
            string diagnosis = Console.ReadLine();

            Console.WriteLine("Enter Prescription:");
            string prescription = Console.ReadLine();

            Console.Write("Enter visit date:");
            string visitDate = Console.ReadLine();

            int recordId = context.MedicalRecords.Count + 1;

            #region Previous Code 
            //Doctor doctor = null;

            //foreach (Doctor item in context.Doctors) // يدور على الدكتور المسؤول عن الموعد اللي فوق 
            //{
            //    if (item.doctorId == appointment.doctorId)
            //    {
            //        doctor = item;
            //        break;
            //    }
            //} 
            #endregion


            context.MedicalRecords.Add(new MedicalRecord // هنا ينشئ السجل الطبي الجديد بعد التشخيص 
            {
                recordId = recordId,
                patientId = appointment.patientId,
                doctorId = appointment.doctorId,
                appointmentId = appointmentId,
                diagnosis = diagnosis,
                prescription = prescription,
                visitDate = appointment.appointmentDate,
                visitFee = fee
            });

            appointment.status = "Completed";

            Console.WriteLine("Medical Record Created");
        }

        public static void PatientMedicalHistoryReport() // 09
        {
            Console.WriteLine("Enter Patient ID:");
            int patientId = Convert.ToInt32(Console.ReadLine());

            Patient patient = context.Patients.FirstOrDefault(p => p.patientId == patientId);

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            List<MedicalRecord> records = context.MedicalRecords.Where(r => r.patientId == patientId).ToList(); //جلب كل سجلات المريض

            if (records.Count == 0)
            {
                Console.WriteLine("No medical records founded.");
                return;
            }

            Console.WriteLine($"Medical History for {patient.patientName}... ");

            records.ForEach(r =>   // تبحث عن الدكتور المرتبط بالسجل
            {
                string doctorName = context.Doctors.Where(d => d.doctorId == r.doctorId)
                                                   .Select(d => d.doctorName)
                                                   .FirstOrDefault() ?? "Unknown"; // إذا لم تجد الدكتور>> بدل null

                Console.WriteLine($"  Record ID   : {r.recordId}");
                Console.WriteLine($"  Visit Date  : {r.visitDate}");
                Console.WriteLine($"  Doctor      : {doctorName}");
                Console.WriteLine($"  Diagnosis   : {r.diagnosis}");
                Console.WriteLine($"  Prescription: {r.prescription}");
                Console.WriteLine($"  Fee         : {r.visitFee}");
                Console.WriteLine("=================================");
            });

            decimal totalEvryVisit = records.Sum(r => r.visitFee);
            Console.WriteLine($"Total Amount Paid = {totalEvryVisit}");

            #region Previous Code 
            //bool found = false;
            //decimal totalEvryVisit = 0; // نعرف متغير جديد .. تكلفة العلاج 

            //foreach (MedicalRecord record in context.MedicalRecords)
            //{
            //    if (record.patientId == patientId)
            //    {
            //        found = true;

            //        string doctorName = "";

            //        foreach (Doctor doctor in context.Doctors)
            //        {
            //            if (doctor.doctorId == record.doctorId) //نطابق اسم الطبيب اللي عالج حسب التقرير
            //            {
            //                doctorName = doctor.doctorName;
            //                break;
            //            }
            //        };

            //Console.WriteLine($"Visit Date: {record.visitDate}");
            //Console.WriteLine($"Doctor: {doctorName}");
            //Console.WriteLine($"Diagnosis: {record.diagnosis}");
            //Console.WriteLine($"Prescription: {record.prescription}");
            //Console.WriteLine($"Fee: {record.visitFee}");
            //Console.WriteLine("===============================");

            //totalEvryVisit += record.visitFee;
            //Console.WriteLine($"Total Amount Paid = {totalEvryVisit}"); 
            #endregion
        }

        public static void DoctorRevenueSummary() // 10 
        {
            if (context.Appointments.Count == 0)
            {
                Console.WriteLine("No Appointment Found");
                return;
            }

            var summary = context.Doctors.Select(d => new
            {
                d.doctorId,
                d.doctorName,
                d.doctorSpecialization,

                completed = context.Appointments.Count(a => a.doctorId == d.doctorId && a.status == "Completed"),

                cancelled = context.Appointments.Count(a => a.doctorId == d.doctorId && a.status == "Cancelled"),

                totalRevenue = context.MedicalRecords.Where(r => r.doctorId == d.doctorId).Sum(r => r.visitFee)
            })

                .OrderByDescending(x => x.totalRevenue) // ترتيب الدكاتره حسب حسب الربح
                .ToList();

            Console.WriteLine($"Doctor ID | Doctor Name | Specialization | Completed | Cancelled | Total Revenue");
            Console.WriteLine("=================================================================================");

            foreach (var x in summary)
            {
                Console.WriteLine($"{x.doctorId} | " +
                                  $"{x.doctorName} | " +
                                  $"{x.doctorSpecialization} | " +
                                  $"{x.completed} | " +
                                  $"{x.cancelled} | " +
                                  $"{x.totalRevenue}");
            }
            #region Previous Code 
            //foreach (Doctor doctor in context.Doctors)
            //{
            //    //نعرف المتغيرات الجديده ولنفرض تبدأ من الصفر 
            //    int completedAppointments = 0;
            //    int cancelledAppointments = 0;
            //    decimal totalRevenue = 0;

            //    foreach (Appointment appointment in context.Appointments) 
            //    {
            //        if (appointment.doctorId == doctor.doctorId) // نطابق اسم الدكتور عشان نقدر نوصل للحاله 
            //        {
            //            if (appointment.status == "Completed")
            //            {
            //                completedAppointments++;
            //            }

            //            if (appointment.status == "Cancelled")
            //            {
            //                cancelledAppointments++;
            //            }
            //        }
            //    }

            //    foreach (MedicalRecord record in context.MedicalRecords)
            //    {
            //        if (record.doctorId == doctor.doctorId) // نفس الشي نطابق اسم الدكتور للوصول إلى إجمالي إيرادات استشاراته
            //        {
            //            totalRevenue += record.visitFee;
            //        }
            //    }

            //    Console.WriteLine($"Doctor Name: {doctor.doctorName}");
            //    Console.WriteLine($"Completed Appointments: {completedAppointments}");
            //    Console.WriteLine($"Cancelled Appointments: {cancelledAppointments}");
            //    Console.WriteLine($"Total Revenue: {totalRevenue}");
            //    Console.WriteLine("===============================");
            //} 
            #endregion
        }

        public static void CalculatePatientDiscount(HospitalContext context) // 11
        {
            Console.WriteLine("Enter Patient ID:");
            int patientId = Convert.ToInt32(Console.ReadLine());

            Patient patient = context.Patients.FirstOrDefault(p => p.patientId == patientId);

            if (patient == null)
            {
                Console.WriteLine("Patient Not Found");
                return;
            }

            int visitsCount = context.MedicalRecords.Count(r => r.patientId == patientId);

            if (visitsCount >= 5)
            {

                MedicalRecord lastRecord = context.MedicalRecords.Where(r => r.patientId == patientId)
                                                                 .OrderByDescending(r => r.recordId)
                                                                 .FirstOrDefault();

                decimal consultationFee = lastRecord.visitFee;

                decimal visitFee = consultationFee - 5;

                Console.WriteLine($"Patient Name = {patient.patientName}");
                Console.WriteLine($"Number Of Visits = {visitsCount}");
                Console.WriteLine($"Original Fee = {consultationFee}");
                Console.WriteLine($"Discount = 5 OMR");
                Console.WriteLine($"{consultationFee} - 5 = {visitFee}");

            }
        }

        public static void DisplayTopDoctor() // 12
        {

            if (context.Doctors.Count == 0)
            {
                Console.WriteLine("No Doctors Found");
                return;
            }

            var topDoctor = context.Doctors
                                   .Select(d => new
                                   {
                                       d.doctorId,
                                       d.doctorName,
                                       TotalAppointments = context.Appointments.Count(a => a.doctorId == d.doctorId)
                                   })
                                   .OrderByDescending(d => d.TotalAppointments)
                                   .FirstOrDefault();

            if (topDoctor == null)
            {
                Console.WriteLine("No Appointments Found");
                return;
            }

            Console.WriteLine("Top Doctor Report");
            Console.WriteLine("====================");
            Console.WriteLine($"Doctor ID: {topDoctor.doctorId}");
            Console.WriteLine($"Doctor Name: {topDoctor.doctorName}");
            Console.WriteLine($"Total Appointments: {topDoctor.TotalAppointments}");
        }
        
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        static void Main(string[] args)
        {

            bool exit = false;
            while (exit == false)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("Welcome to Hospital Management System");
                Console.WriteLine("======================================");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("\n1. Register Patient");
                Console.WriteLine("2. Add New Doctor");
                Console.WriteLine("3. View All Patients");
                Console.WriteLine("4. View Doctors By Specialization");
                Console.WriteLine("5. Add Available Slot");
                Console.WriteLine("6. Book an Appointment");
                Console.WriteLine("7. Cancel Appointment");
                Console.WriteLine("8. Create Medical Record");
                Console.WriteLine("9. Patient Medical History Report");
                Console.WriteLine("10. Doctor Revenue Summary");
                Console.WriteLine("0. Exit");
                Console.WriteLine("======================================");

                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        PatientRegistration();
                        break;

                    case 2:
                        AddNewDoctor();
                        break;

                    case 3:
                        ViewAllPatients();
                        break;

                    case 4:
                        ViewAllDoctorsbySpecialization();
                        break;

                    case 5:
                        AddAvailableSlot();
                        break;

                    case 6:
                        BookAppointment();
                        break;

                    case 7:
                        CancelAppointment();
                        break;

                    case 8:
                        CreateMedicalRecord();
                        break;

                    case 9:
                        PatientMedicalHistoryReport();
                        break;

                    case 10:
                        DoctorRevenueSummary();
                        break;

                    case 0:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        break;

                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}