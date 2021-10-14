using UniRx;

public static class SubjectExt 
{
    public static Subject<T> ResetSubject<T>(this Subject<T> target)
    {
        target.isStopped = false;
        return target;
    }
}
