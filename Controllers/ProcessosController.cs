using DesafioFSBR.Data;
using DesafioFSBR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList.EF;

namespace DesafioFSBR.Controllers
{
    public class ProcessosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProcessosController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Processos
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var query = _context.Processos
            .OrderBy(p => p.NPU)
            .AsQueryable();

            var processosPaginados = await query.ToPagedListAsync(pageNumber, pageSize);

            return View(processosPaginados);
        }

        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // GET: Processos/Create
        public async Task<IActionResult> Create()
        {
            var ufs = await ObterUfs_IBGE();
            if (ufs != null && ufs.Any())
            {
                ViewBag.UFs = ufs;
            }
            else
            {
                ViewBag.UFs = new List<string>();
            }
            return View();
        }

        private async Task<List<string>> ObterUfs_IBGE()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://servicodados.ibge.gov.br/api/v1/localidades/estados/");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var ufs = JsonConvert.DeserializeObject<List<dynamic>>(json);
                return ufs.Select(uf => (string)uf.sigla).ToList();
            }

            return new List<string>();
        }

        public async Task<JsonResult> ObterMunicipios(string uf)
        {
            if (string.IsNullOrEmpty(uf))
                return Json(new List<object>());

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{uf}/municipios");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var municipios = JsonConvert.DeserializeObject<List<dynamic>>(json);
                var result = municipios.Select(m => new
                {
                    Nome = m.nome.ToString(),
                    Codigo = m.id.ToString()
                }).ToList();
                return Json(result);
            }

            return Json(new List<object>());
        }


        // POST: Processos/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProcesso,NPU,DataCadastro,DataVisualizacao,UF,Municipio")] Processo processo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processo);
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processos.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            var ufs = await ObterUfs_IBGE();
            if (ufs != null && ufs.Any())
            {
                ViewBag.UFs = ufs;
            }
            else
            {
                ViewBag.UFs = new List<string>();
            }

            return View(processo);
        }

        // POST: Processos/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeProcesso,NPU,DataVisualizacao,UF,Municipio")] Processo processo)
        {
            if (id != processo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var processoExistente = await _context.Processos.FindAsync(id);
                    if (processoExistente == null)
                    {
                        return NotFound();
                    }
                    processo.DataCadastro = processoExistente.DataCadastro;

                    _context.Entry(processoExistente).CurrentValues.SetValues(processo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoExists(processo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(processo);
        }

        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // POST: Processos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo != null)
            {
                _context.Processos.Remove(processo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoExists(int id)
        {
            return _context.Processos.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarVisualizacao(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            processo.DataVisualizacao = DateTime.Now;

            try
            {
                _context.Update(processo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Erro ao atualizar o registro.");
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
