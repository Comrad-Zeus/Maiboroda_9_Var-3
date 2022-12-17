// Майборода А.А. - Вар 3 - Высокий
const int internetClientSessionsNumber = 100;
var clientLogins = new[] { "smoothbronx", "alexanderLis", "horta" };

var internetClientsSessions = new InternetClientSession[internetClientSessionsNumber];
var currentDate = DateOnly.FromDateTime(DateTime.Now);

var random = new Random();

for (var sessionIndex = 0; sessionIndex < internetClientSessionsNumber; sessionIndex++)
    internetClientsSessions[sessionIndex] = CreateInternetClientSession();

Main();

void Main()
{
    PrintWarning();
    Console.WriteLine();
    FirstTask();
    Console.WriteLine();
    SecondTask();
    Console.WriteLine();
    ThirdTask();
}

void FirstTask()
{
    Console.Write($"Введите номер интересующего вас месяца: ");
    var selectedMonthNumber = int.Parse(Console.ReadLine()!);
    var requiredSessionsNumber = internetClientsSessions
        .Where(session => session.onlineSessionDate.Year == currentDate.Year - 1)
        .Count(session => session.onlineSessionDate.Month == selectedMonthNumber);
    Console.WriteLine($"Количество сеансов в период {selectedMonthNumber.ToString().PadLeft(2, '0')}.{currentDate.Year - 1} составляет: {requiredSessionsNumber}");
}

void SecondTask()
{
    Console.Write($"Введите интересующую вас дату (DD.MM.YYYY): ");
    var dateSegments = Console.ReadLine()!.Split('.');
    var selectedDate = new DateOnly(
        int.Parse(dateSegments[2]),
        int.Parse(dateSegments[1]),
        int.Parse(dateSegments[0]));

    try
    {
        var selectedSession = internetClientsSessions
            .Where(session => selectedDate == session.onlineSessionDate)
            .MaxBy(session => session.GetSessionDuration());
        Console.WriteLine($"Макисмальная длительность сеанса в этот день ({selectedDate}): {selectedSession.GetSessionDuration()}");
    }
    catch
    {
        Console.WriteLine("В данный день не было сеансов");
    }
}

void ThirdTask()
{
    Console.WriteLine("Третья подзадача связана с файловой системой, поэтому была справедливо пропущена");
}

void PrintWarning()
{
    Console.WriteLine("Чтобы увеличить плотность данных, было урезано количество дней");
    Console.WriteLine("Выбор (1,.., 10) числа (10,.., 12) месяца текущего или прошлого года.");
}

InternetClientSession CreateInternetClientSession()
{
    var sessionEndTime = new TimeOnly(random.Next(3, 24), random.Next(0, 60));
    var sessionStartTime = new TimeOnly(random.Next(sessionEndTime.Hour - 3, sessionEndTime.Hour + 1), random.Next(0, sessionEndTime.Minute + 1));
    var clientLogin = clientLogins[random.Next(0, clientLogins.Length)];
    var sessionDate = new DateOnly(random.Next(currentDate.Year - 1, currentDate.Year + 1), random.Next(10, 13), random.Next(1, 11));

    var receivedDataSize = random.Next(5_000, 100_000);
    var sentDataSize = random.Next(5_000, 100_000);

    return new InternetClientSession(
        clientLogin, sessionDate,
        sessionStartTime, sessionEndTime,
        receivedDataSize, sentDataSize);
}

internal struct InternetClientSession
{
    public string clientLogin;
    public DateOnly onlineSessionDate;
    public TimeOnly startOnlineSessionTime;
    public TimeOnly endOnlineSessionTime;

    public int receivedDataSize;
    public int sentDataSize;

    public InternetClientSession(string clientLogin, DateOnly onlineSessionDate, TimeOnly startOnlineSessionTime,
        TimeOnly endOnlineSessionTime, int receivedDataSize, int sentDataSize)
    {
        this.clientLogin = clientLogin;
        this.onlineSessionDate = onlineSessionDate;
        this.startOnlineSessionTime = startOnlineSessionTime;
        this.endOnlineSessionTime = endOnlineSessionTime;
        this.receivedDataSize = receivedDataSize;
        this.sentDataSize = sentDataSize;
    }

    public TimeOnly GetSessionDuration()
    {
        return TimeOnly.FromTimeSpan(endOnlineSessionTime - startOnlineSessionTime);
    }
}
