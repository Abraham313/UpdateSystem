﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeElements.UpdateSystem.Utilities
{
    public class CountingStreamWrapper : Stream
    {
        private readonly Stream _baseStream;

        public CountingStreamWrapper(Stream baseStream)
        {
            _baseStream = baseStream;
        }

        public int TotalDataRead { get; private set; }
        public int LastDataRead { get; private set; }

        public override bool CanRead => _baseStream.CanRead;
        public override bool CanSeek => _baseStream.CanSeek;
        public override bool CanWrite => _baseStream.CanWrite;
        public override long Length => _baseStream.Length;

        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = _baseStream.Read(buffer, offset, count);
            TotalDataRead += read;
            LastDataRead = read;
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var read = await _baseStream.ReadAsync(buffer, offset, count, cancellationToken);
            TotalDataRead += read;
            LastDataRead = read;
            return read;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _baseStream.WriteAsync(buffer, offset, count, cancellationToken);
        }
    }
}