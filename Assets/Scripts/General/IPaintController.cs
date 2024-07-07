public interface IPaintController
{
    float Interval { get; set; }

    void Continue();
    void Pause();
}