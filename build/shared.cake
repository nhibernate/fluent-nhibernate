public class MsBuildShared
{
	public string SharedMsBuildFile { get; private set; }
	public string NHibernatePackageVersion { get; private set; }
	
	public static MsBuildShared GetShared(
        ICakeContext context,
		string sharedFile)
    {
		if (context == null)
        {
            throw new ArgumentNullException("context");
        }
		if (string.IsNullOrEmpty(sharedFile))
        {
            throw new ArgumentNullException("sharedFile");
        }
		
		var nHibernatePackageVersion = context.XmlPeek(sharedFile, "/Project/PropertyGroup/NHibernatePackageVersion");
		
		return new MsBuildShared()
		{
			SharedMsBuildFile = sharedFile,
			NHibernatePackageVersion = nHibernatePackageVersion
		};
	}
}