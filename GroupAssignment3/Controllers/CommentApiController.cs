using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Helper;
using GroupAssignment3.Identity;
using GroupAssignment3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GroupAssignment3.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CommentApiController : ControllerBase
    {
        private readonly ProductCommentService commentService;
        private readonly IHttpContextAccessor httpContext;

        public CommentApiController(ProductCommentService commentService, IHttpContextAccessor httpContext)
        {
            this.commentService = commentService;
            this.httpContext = httpContext;
        }

        [HttpGet("v1/comment/product/{productId}")]
        public IActionResult GetComments(string productId)
        {
            var comments = this.commentService.GetProductComment(productId);
            return Ok(RestFormat.CommentEntity(comments));
        }

        [HttpDelete("v1/comment/product/{commentId}")]
        [Authorize(Policy = IdentityData.ScopeUserPolicyName)]
        public IActionResult DeleteComment(string commentId)
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            this.commentService.DeleteComment(userId, commentId);
            return Ok(new { message = "Comment correctly removed" });
        }

        [HttpPost("v1/comment/product")]
        [Authorize(Policy = IdentityData.ScopeUserPolicyName)]
        public IActionResult MakeComment([FromBody] ProductCommentDto comment)
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            this.commentService.MakeComment(userId, comment);
            return Ok(new { message = "Comment correctly added" });
        }
    }
}