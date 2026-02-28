const fs = require('fs');
const path = require('path');

function walkDir(dir, callback) {
    fs.readdirSync(dir).forEach(f => {
        let dirPath = path.join(dir, f);
        let isDirectory = fs.statSync(dirPath).isDirectory();
        isDirectory ? walkDir(dirPath, callback) : callback(path.join(dir, f));
    });
}

walkDir(path.join(__dirname, 'src'), function (filePath) {
    if (filePath.endsWith('.tsx') || filePath.endsWith('.ts')) {
        let content = fs.readFileSync(filePath, 'utf-8');
        let original = content;

        // 1. Replace the import
        // import { CustomizerContext } from "@/store/customizerContext";
        // or relative like import { CustomizerContext } from '@/store/customizerContext';
        content = content.replace(/import\s*\{\s*CustomizerContext\s*\}\s*from\s*['"](?:[^'"]*\/)?store\/customizerContext['"];?\r?\n?/g, '');

        // Add import { useCustomizer } from '@/hooks/useCustomizer'; if needed
        if (original !== content && !content.includes('useCustomizer')) {
            // Insert after the last import, or at top
            content = `import { useCustomizer } from '@/hooks/useCustomizer';\n` + content;
        }

        // 2. Replace useContext(CustomizerContext) with useCustomizer()
        content = content.replace(/useContext\(\s*CustomizerContext\s*\)/g, 'useCustomizer()');

        // 3. Remove React.useContext if used
        content = content.replace(/React\.useContext\(\s*CustomizerContext\s*\)/g, 'useCustomizer()');

        if (original !== content) {
            fs.writeFileSync(filePath, content, 'utf-8');
            console.log('Updated', filePath);
        }
    }
});
