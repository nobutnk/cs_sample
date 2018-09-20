Class GitClientUtils {

  /**
   * repository取得するところまでは想像。
   */
  public File getFileFromGitRepository(string repositoryName, string url)
  {
    // このあたりは適当
    Microsoft.VisualStudio.Services.Common.WindowsCredential credential
     = new Microsoft.VisualStudio.Services.Common.WindowsCredential(new NetworkCredential(XXX));

    // Credencial取得できる？
    var vssCredentials = new VssCredentials(credential);

    // GitHttpClient使えるの前提
    // 対象ファイルのURLをここで渡す
    GitHttpClient client = new GitHttpClient(new Uri(url), vssCredentials);

    try {
        // repositoryは取得できる前提
        var repositories = await client.GetRepositoriesAsync();
        var repository = repositories.FirstOrDefault(i => i.Name == repositoryName);
        var repositoryId = repository.Id;

        // フォルダ配下取得するときは、GetItemsAsyncで、scopePathをフォルダ単位で設定
        var items = await client.GetItemsAsync(repositoryId, scopePath:"/Solution1/ConsoleApplication1",recursionLevel:VersionControlRecursionType.Full);
        foreach (var item in items)
        {
            if(!item.IsFolder)
            {
                var blob = await client.GetBlobContentAsync(repositoryId, item.ObjectId, ...);
                // あとは省略。Streamからファイル化してください。
            }
        }

        // ファイル単位のときはこんなかんじのはず.var blobはTask<Stream>のこと
        var item = await client.GetItemAsync(repositoryId, path:"ファイルパス設定", recursionLevel:VersionControlRecursionType.Full);
        var blob = await client.GetBlobContentAsync(repositoryId, item.ObjectId, ...);
        // あとは省略。Streamからファイル化してください。

        // 別手法。これで１行で取得できそう。引数はもっとある。
        // var blobはTask<Stream>のこと
        var blob = await client.GetItemContentAsync(repositoryId, path:"ファイルパス設定", recursionLevel:VersionControlRecursionType.Full);
        // あとは省略。Streamからファイル化してください。
    }
    catch(Exception e)
    {
      // 例外処理
    }
  }
}
