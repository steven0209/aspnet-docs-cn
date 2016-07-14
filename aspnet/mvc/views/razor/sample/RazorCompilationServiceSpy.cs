using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;

//namespace RazorSample
//{
//    public class RazorCompilationServiceSpy : IRazorCompilationService
//    {
//        private IRazorCompilationService _inner;
//        private IList<CompileEntry> _log;

//        public RazorCompilationServiceSpy(ICompilationService compilationService,
//                                          IMvcRazorHost razorHost,
//                                          IOptions<RazorViewEngineOptions> options)
//        {
//            _inner = new RazorCompilationService(compilationService, razorHost, options);
//            _log = new List<CompileEntry>();
//        }

//        public CompilationResult Compile(RelativeFileInfo fileInfo)
//        {
//            var result = _inner.Compile(fileInfo);
//            _log.Add(new CompileEntry { FileInfo = fileInfo, Result = result });
//            return result;
//        }

//        public IEnumerable<CompileEntry> CompilationLog
//        {
//            get
//            {
//                return _log;
//            }
//        }

//        public class CompileEntry
//        {
//            public RelativeFileInfo FileInfo { get; set; }
//            public CompilationResult Result { get; set; }
//        }
//    }
//}

public class MyCompliationService : ICompilationService
{
    CompilationResult ICompilationService.Compile(RelativeFileInfo fileInfo, string compilationContent)
    {
        throw new NotImplementedException();
    }
}
