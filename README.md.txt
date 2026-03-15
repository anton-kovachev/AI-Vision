
---

## 🎯 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/scan-receipt` | Upload receipt image for AI analysis |
| `GET` | `/` | Swagger UI documentation |
| `GET` | `/health` | Health check endpoint |
| `GET` | `/alive` | Liveness probe |
| `GET` | `/ready` | Readiness probe |

---

## 🐛 Troubleshooting

### Issue: Model takes too long to download
**Solution**: Ensure stable internet connection. Monitor progress in Aspire Dashboard under the Ollama container logs.

### Issue: Timeout errors during inference
**Solution**: Increase timeout in `Program.cs` or use a smaller/faster model like `llava:7b`.

### Issue: Docker not running
**Solution**: Start Docker Desktop before running the application.

### Issue: Port conflicts
**Solution**: Aspire automatically assigns ports. Check the Aspire Dashboard for actual service URLs.

---

## 🔮 Future Enhancements

- [ ] Multi-language receipt support
- [ ] Batch processing for multiple receipts
- [ ] Export to Excel/CSV
- [ ] Integration with accounting software (QuickBooks, Xero)
- [ ] OCR fallback for non-AI extraction
- [ ] Receipt duplicate detection
- [ ] Cloud deployment templates (Azure Container Apps)

---

## 📚 Learn More

- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Ollama Official Site](https://ollama.ai/)
- [LLaMA 3.2 Model Card](https://huggingface.co/meta-llama/Llama-3.2-Vision)
- [OllamaSharp GitHub](https://github.com/awaescher/OllamaSharp)

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 👨‍💻 Author

**Your Name**
- GitHub: [@yourusername](https://github.com/yourusername)
- LinkedIn: [Your Profile](https://linkedin.com/in/yourprofile)

---

## ⭐ Show Your Support

Give a ⭐️ if this project helped you!

---

**Built with ❤️ using .NET Aspire and AI**