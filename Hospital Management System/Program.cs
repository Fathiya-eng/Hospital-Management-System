using Hospital_Management_System.Models;
namespace Hospital_Management_System
{
    public class Program
    {
        public static void PatientRegistration(HospitalContext context) // 01
        {
           int patientId = context.Patients.Count + 1;

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

            Console.WriteLine($"Patient registered successfully.. ID = {patientId}");

        }

        public static void AddNewDoctor(HospitalContext context) // 02 
        {
            int doctorId = context.Doctors.Count + 1;

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

            context.Doctors.Add(new Doctor
            {
                doctorId = doctorId,
                doctorName = doctorName,
                doctorSpecialization = doctorSpecialization,
                doctorPhone = doctorPhone,
                doctorEmail = doctorEmail,
                consultationFee =consultationFee

            });

            Console.WriteLine($"Doctor registered successfully.. ID: {doctorId}");
        }

        public static void ViewAllPatients(HospitalContext context) // 03 
        {
            foreach (Patient patient in context.Patients) 
            {
                Console.WriteLine($"patient Id: {patient.patientId}");
                Console.WriteLine($"patient Name: {patient.patientName}");
                Console.WriteLine($"patient Age: {patient.patientAge}");
                Console.WriteLine($"patient Gender: {patient.patientGender}");
                Console.WriteLine($"patient Phone: {patient.patientPhone}");
                Console.WriteLine($"patient Email: {patient.patientEmail}");
                Console.WriteLine($"patient Blood Type: {patient.patientBloodType}");
                Console.WriteLine("===============================");
            }

            if (context.Patients.Count == 0)
            {
                Console.WriteLine("No Patients Found");
                return;
            }
        }

        public static void ViewAllDoctorsbySpecialization(HospitalContext context) // 04
        {
            Console.WriteLine("Enter Specialization:");
            string doctorSpecialization = Console.ReadLine();

            bool found = false;
            foreach (Doctor doctor in context.Doctors)
            {
                if (doctor.doctorSpecialization == doctorSpecialization)
                {
                    Console.WriteLine($"Doctor Name: {doctor.doctorName}");
                    Console.WriteLine($"consultation Fee: {doctor.consultationFee}");
                    Console.WriteLine("===============================");

                    found = true;
                }
            }

            if (found == false)
            {
                Console.WriteLine("No Doctors Found");
            }
        }

        public static void AddAvailableSlot(HospitalContext context) // 05 
        {
            int slotId = context.AvailableSlots.Count + 1;

            Console.WriteLine("Enter Doctor ID:");
            int doctorId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Date:");
            string slotDate = Console.ReadLine();

            Console.WriteLine("Enter Time:");
            string slotTime = Console.ReadLine();

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

        public static void BookAppointment(HospitalContext context) // 06 
        {
            int appointmentId = context.Appointments.Count + 1;

            Console.WriteLine("Enter Patient ID:");
            int patientId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Doctor ID:");
            int doctorId = Convert.ToInt32(Console.ReadLine());

            bool slotFound = false;

            foreach (AvailableSlot slot in context.AvailableSlots) // AvailableSlot ==> slot 
            {
                if (slot.doctorId == doctorId && slot.isBooked == false)
                {
                    Console.WriteLine($"Slot ID: {slot.slotId}");
                    Console.WriteLine($"Doctor ID: {slot.doctorId}");
                    Console.WriteLine($"{slot.slotDate} , {slot.slotTime}");
                    Console.WriteLine("===============================");

                    slotFound = true;
                }
            }

            if (slotFound == false)
            {
                Console.WriteLine("No Available Slots");
                return;
            }
            /////////////////////////////////////

            Console.WriteLine("Choose Slot ID:");
            int slotId = Convert.ToInt32(Console.ReadLine()); // مثلا المريض اختار 2 ، الوقت اللي ناسبه

            AvailableSlot availableSlot = null; // متغير فارغ عشان يخزن بعدين الsolt اللي اختاره المريض 

            foreach (AvailableSlot slot in context.AvailableSlots)
            {
                if (slot.slotId == slotId) // 1==2 X , 2==2 /
                {
                    availableSlot = slot; 
                    break;
                }
            }

            context.Appointments.Add(new Appointment 
            {
                appointmentId = appointmentId,
                patientId = patientId,
                doctorId = doctorId,
                appointmentDate = availableSlot.slotDate,
                appointmentTime = availableSlot.slotTime,
                status = "Booked"
            });

            availableSlot.isBooked = true; // تم حجز موعد المريض 

            Console.WriteLine("Appointment Booked Successfully");
        }

        public static void CancelAppointment(HospitalContext context) // 07 
        {
            Console.WriteLine("Enter Appointment ID:");
            int appointmentId = Convert.ToInt32(Console.ReadLine());

            Appointment appointment = null;

            foreach (Appointment item in context.Appointments)
            {
                if (item.appointmentId == appointmentId)
                {
                    appointment = item;
                    break;
                }
            }

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

            appointment.status = "Cancelled";

            foreach (AvailableSlot slot in context.AvailableSlots)
            {
                if (slot.doctorId == appointment.doctorId &&
                    slot.slotDate == appointment.appointmentDate &&
                    slot.slotTime == appointment.appointmentTime)
                {
                    slot.isBooked = false;
                }
            }

            Console.WriteLine("Appointment Cancelled");
        }

        public static void CreateMedicalRecord(HospitalContext context) // 08 
        {
            int recordId = context.MedicalRecords.Count + 1;

            Console.WriteLine("Enter Appointment ID:");
            int appointmentId = Convert.ToInt32(Console.ReadLine());

            Appointment appointment = null;

            foreach (Appointment item in context.Appointments)
            {
                if (item.appointmentId == appointmentId)
                {
                    appointment = item;
                    break;
                }
            }

            if (appointment == null)
            {
                Console.WriteLine("Appointment Not Found");
                return;
            }

            Console.WriteLine("Enter Diagnosis:");
            string diagnosis = Console.ReadLine();

            Console.WriteLine("Enter Prescription:");
            string prescription = Console.ReadLine();

            Doctor doctor = null;

            foreach (Doctor item in context.Doctors)
            {
                if (item.doctorId == appointment.doctorId)
                {
                    doctor = item;
                    break;
                }
            }

            context.MedicalRecords.Add(new MedicalRecord
            {
                recordId = recordId,
                patientId = appointment.patientId,
                doctorId = appointment.doctorId,
                appointmentId = appointmentId,
                diagnosis = diagnosis,
                prescription = prescription,
                visitDate = appointment.appointmentDate,
                visitFee = doctor.consultationFee
            });

            appointment.status = "Completed";

            Console.WriteLine("Medical Record Created");
        }

        public static void PatientMedicalHistoryReport(HospitalContext context) // 09
        {
            Console.WriteLine("Enter Patient ID:");
            int patientId = Convert.ToInt32(Console.ReadLine());

            bool found = false;
            decimal totalEvryVisit = 0; // نعرف متغير جديد .. تكلفة العلاج 

            foreach (MedicalRecord record in context.MedicalRecords)
            {
                if (record.patientId == patientId)
                {
                    found = true;

                    string doctorName = "";

                    foreach (Doctor doctor in context.Doctors)
                    {
                        if (doctor.doctorId == record.doctorId) //نطابق اسم الطبيب اللي عالج حسب التقرير
                        {
                            doctorName = doctor.doctorName;
                            break;
                        }
                    }

                    Console.WriteLine($"Visit Date: {record.visitDate}");
                    Console.WriteLine($"Doctor: {doctorName}");
                    Console.WriteLine($"Diagnosis: {record.diagnosis}");
                    Console.WriteLine($"Prescription: {record.prescription}");
                    Console.WriteLine($"Fee: {record.visitFee}");
                    Console.WriteLine("===============================");

                    totalEvryVisit += record.visitFee;
                    Console.WriteLine($"Total Amount Paid = {totalEvryVisit}");
                }
            }

            if (found == false)
            {
                Console.WriteLine("No Medical Records Found");
                return;
            }
        }

        public static void DoctorRevenueSummary(HospitalContext context) // 10 
        {
            if (context.Doctors.Count == 0)
            {
                Console.WriteLine("No Doctors Found");
                return;
            }

            foreach (Doctor doctor in context.Doctors)
            {
                //نعرف المتغيرات الجديده ولنفرض تبدأ من الصفر 
                int completedAppointments = 0;
                int cancelledAppointments = 0;
                decimal totalRevenue = 0;

                foreach (Appointment appointment in context.Appointments) 
                {
                    if (appointment.doctorId == doctor.doctorId) // نطابق اسم الدكتور عشان نقدر نوصل للحاله 
                    {
                        if (appointment.status == "Completed")
                        {
                            completedAppointments++;
                        }

                        if (appointment.status == "Cancelled")
                        {
                            cancelledAppointments++;
                        }
                    }
                }

                foreach (MedicalRecord record in context.MedicalRecords)
                {
                    if (record.doctorId == doctor.doctorId) // نفس الشي نطابق اسم الدكتور للوصول إلى إجمالي إيرادات استشاراته
                    {
                        totalRevenue += record.visitFee;
                    }
                }

                Console.WriteLine($"Doctor Name: {doctor.doctorName}");
                Console.WriteLine($"Completed Appointments: {completedAppointments}");
                Console.WriteLine($"Cancelled Appointments: {cancelledAppointments}");
                Console.WriteLine($"Total Revenue: {totalRevenue}");
                Console.WriteLine("===============================");
            }
        }

        static void Main(string[] args)
        {
            HospitalContext context = new HospitalContext();
            context.Appointments = new List<Appointment>();
            context.Patients = new List<Patient>();
            context.Doctors = new List<Doctor>();
            context.Doctors = new List<Doctor>();
            context.AvailableSlots = new List<AvailableSlot>();
            context.MedicalRecords = new List<MedicalRecord>();

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
                        PatientRegistration(context);
                        break;

                    case 2:
                        AddNewDoctor(context);
                        break;

                    case 3:
                        ViewAllPatients(context);
                        break;

                    case 4:
                        ViewAllDoctorsbySpecialization(context);
                        break;

                    case 5:
                        AddAvailableSlot(context);
                        break;

                    case 6:
                        BookAppointment(context);
                        break;

                    case 7:
                        CancelAppointment(context);
                        break;

                    case 8:
                        CreateMedicalRecord(context);
                        break;

                    case 9:
                        PatientMedicalHistoryReport(context);
                        break;

                    case 10:
                        DoctorRevenueSummary(context);
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
