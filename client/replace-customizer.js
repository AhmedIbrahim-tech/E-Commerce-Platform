const fs = require('fs');
const path = require('path');

function processDirectory(dir) {
    const files = fs.readdirSync(dir);
    for (const file of files) {
        const fullPath = path.join(dir, file);
        if (fs.statSync(fullPath).isDirectory()) {
            processDirectory(fullPath);
        } else if (fullPath.endsWith('.ts') || fullPath.endsWith('.tsx')) {
            let content = fs.readFileSync(fullPath, 'utf8');
            let newContent = content;

            // Replace import
            newContent = newContent.replace(/import\s+{\s*CustomizerContext\s*}\s+from\s+['"]@\/store\/customizerContext['"];?/g, 'import { useCustomizer } from "@/hooks/useCustomizer";');

            // Replace React.useContext
            newContent = newContent.replace(/React\.useContext\(CustomizerContext\)/g, 'useCustomizer()');

            // Replace useContext
            newContent = newContent.replace(/useContext\(CustomizerContext\)/g, 'useCustomizer()');

            if (content !== newContent) {
                fs.writeFileSync(fullPath, newContent);
                console.log(`Updated hooks in ${fullPath}`);
            }
        }
    }
}

processDirectory(path.join(__dirname, 'src'));
console.log('Hook Replacement Done!');
